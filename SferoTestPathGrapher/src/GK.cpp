/******************************************************************
 Grafika komputerowa, œrodowisko MS Windows - program  przyk³adowy
 *****************************************************************/

#include <windows.h>
#include <gdiplus.h>
#include <math.h>
using namespace Gdiplus;

//deklaracja funkcji obslugi okna
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

#define ROZMIAR 5
#define LICZBA_PUNKTOW 4
bool przesuwamy_slider = false;
bool ruszamy_krzywa = false;
int idx = -1;
POINT Bezier[LICZBA_PUNKTOW] = { {200,200},{400,400},{600,450},{800,200} };//,{500,500},{800,100} };

typedef struct Slider {
	POINT pnt1;
	POINT pnt2;
	POINT cursor;
	float val; // od 0 do 1?
};

Slider slider = { {100,100},{300,100}, {100,100},0 };

void Rysuj_Slider(HDC kontekst) {
	HBRUSH pioro = CreateSolidBrush(RGB(255, 0, 0));
	SelectObject(kontekst, pioro);

	MoveToEx(kontekst, slider.pnt1.x, slider.pnt1.y, NULL);
	LineTo(kontekst, slider.pnt2.x, slider.pnt2.y);

	Arc(kontekst, slider.cursor.x - ROZMIAR,
				  slider.cursor.y - ROZMIAR,
				  slider.cursor.x + ROZMIAR,
		          slider.cursor.y + ROZMIAR,
				  0,0,0,0);
}

double Silnia(double a) {
	double wynik = 1;
	for (int i = 1; i <= a; i++) {
		wynik *= i;
	}
	return wynik;
}

double Symbol_Newtona(double a, double b) {
	return Silnia(a) / (Silnia(a-b)*Silnia(b));
}

void Rysuj_Algorytm_Beziera(HDC *kontekst, POINT punkt[], int lb) {
	HBRUSH pioro = CreateSolidBrush(RGB(10*lb, 255, 0));	
	SelectObject(*kontekst,pioro);

	if (lb >= 2) {
		POINT* punkty = new POINT[lb];
		//rysowanie punktow
		for (int i = 0; i < lb; i++) {			
			Arc(*kontekst, punkt[i].x - ROZMIAR,
				punkt[i].y - ROZMIAR,
				punkt[i].x + ROZMIAR,
				punkt[i].y + ROZMIAR,
				0, 0, 0, 0);
		}
		//obliczanie nowych punktow
		for (int i = 0; i < lb - 1; i++) {
			punkty[i].x = (punkt[i + 1].x - punkt[i].x) * slider.val + punkt[i].x;
			punkty[i].y = (punkt[i + 1].y - punkt[i].y) * slider.val + punkt[i].y;

			//rysowanie lini
			MoveToEx(*kontekst, punkt[i].x, punkt[i].y, NULL);
			LineTo(*kontekst, punkt[i + 1].x, punkt[i + 1].y);
		}


		Rysuj_Algorytm_Beziera(kontekst, punkty, lb - 1);
		delete[lb] punkty;
	}
	else {
		Arc(*kontekst, punkt[0].x - ROZMIAR,
			punkt[0].y - ROZMIAR,
			punkt[0].x + ROZMIAR,
			punkt[0].y + ROZMIAR,
			0, 0, 0, 0);
	}


}

double POT(double x, double y) {
	if (x == 0 && y == 0) {
		return 1;
	}
	double wynik = 1;
	while (y >= 1) {
		wynik *= x;
		y--;
	}
	return wynik;
}


void Rysuj_Pikselowo_Beziera(HDC kontekst) {
	double t = 0;
	double dt = 0.001;
	double xt;
	double yt;
	
	while (t <= 1) {
		xt = 0;
		yt = 0;
		for (int i = 0; i < LICZBA_PUNKTOW; i++) {
			
			xt += Symbol_Newtona(LICZBA_PUNKTOW-1, i) * POT(t, i) * POT(1 - t, LICZBA_PUNKTOW-1 - i) * Bezier[i].x;
			yt += Symbol_Newtona(LICZBA_PUNKTOW-1, i) * POT(t, i) * POT(1 - t, LICZBA_PUNKTOW-1 - i) * Bezier[i].y;			
		}
		MoveToEx(kontekst, xt, yt, NULL);
		LineTo(kontekst, xt+1, yt+1);

		t += dt;
	}
}

//funkcja Main - dla Windows
 int WINAPI WinMain(HINSTANCE hInstance,
               HINSTANCE hPrevInstance,
               LPSTR     lpCmdLine,
               int       nCmdShow)
{
	MSG meldunek;		  //innymi slowy "komunikat"
	WNDCLASS nasza_klasa; //klasa g³ównego okna aplikacji
	HWND okno;
	static char nazwa_klasy[] = "Podstawowa";
	
	GdiplusStartupInput gdiplusParametry;// parametry GDI+; domyœlny konstruktor wype³nia strukturê odpowiednimi wartoœciami
	ULONG_PTR	gdiplusToken;			// tzw. token GDI+; wartoœæ uzyskiwana przy inicjowaniu i przekazywana do funkcji GdiplusShutdown
   
	// Inicjujemy GDI+.
	GdiplusStartup(&gdiplusToken, &gdiplusParametry, NULL);

	//Definiujemy klase g³ównego okna aplikacji
	//Okreslamy tu wlasciwosci okna, szczegoly wygladu oraz
	//adres funkcji przetwarzajacej komunikaty
	nasza_klasa.style         = CS_HREDRAW | CS_VREDRAW | CS_DBLCLKS;
	nasza_klasa.lpfnWndProc   = WndProc; //adres funkcji realizuj¹cej przetwarzanie meldunków 
 	nasza_klasa.cbClsExtra    = 0 ;
	nasza_klasa.cbWndExtra    = 0 ;
	nasza_klasa.hInstance     = hInstance; //identyfikator procesu przekazany przez MS Windows podczas uruchamiania programu
	nasza_klasa.hIcon         = 0;
	nasza_klasa.hCursor       = LoadCursor(0, IDC_ARROW);
	nasza_klasa.hbrBackground = (HBRUSH) GetStockObject(GRAY_BRUSH);
	nasza_klasa.lpszMenuName  = "Menu" ;
	nasza_klasa.lpszClassName = nazwa_klasy;

    //teraz rejestrujemy klasê okna g³ównego
    RegisterClass (&nasza_klasa);
	
	/*tworzymy okno g³ówne
	okno bêdzie mia³o zmienne rozmiary, listwê z tytu³em, menu systemowym
	i przyciskami do zwijania do ikony i rozwijania na ca³y ekran, po utworzeniu
	bêdzie widoczne na ekranie */
 	okno = CreateWindow(nazwa_klasy, "Grafika komputerowa", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
						CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, NULL, NULL, hInstance, NULL);
	
	
	/* wybór rozmiaru i usytuowania okna pozostawiamy systemowi MS Windows */
   	ShowWindow (okno, nCmdShow) ;


	//odswiezamy zawartosc okna
	UpdateWindow (okno) ;
	// G£ÓWNA PÊTLA PROGRAMU
	while (GetMessage(&meldunek, NULL, 0, 0))
     /* pobranie komunikatu z kolejki; funkcja GetMessage zwraca FALSE tylko dla
	 komunikatu WM_QUIT; dla wszystkich pozosta³ych komunikatów zwraca wartoœæ TRUE */
	{
		TranslateMessage(&meldunek); // wstêpna obróbka komunikatu		
		DispatchMessage(&meldunek);  // przekazanie komunikatu w³aœciwemu adresatowi (czyli funkcji obslugujacej odpowiednie okno)		
	}

	GdiplusShutdown(gdiplusToken);
	
	return (int)meldunek.wParam;
}

/********************************************************************
FUNKCJA OKNA realizujaca przetwarzanie meldunków kierowanych do okna aplikacji*/
LRESULT CALLBACK WndProc (HWND okno, UINT kod_meldunku, WPARAM wParam, LPARAM lParam)
{
	HMENU mPlik, mInfo, mGlowne;
    
/* PONI¯SZA INSTRUKCJA DEFINIUJE REAKCJE APLIKACJI NA POSZCZEGÓLNE MELDUNKI */
	switch (kod_meldunku) 
	{
	case WM_CREATE:  //meldunek wysy³any w momencie tworzenia okna
		mPlik = CreateMenu();
		AppendMenu(mPlik, MF_STRING, 100, "&Zapiszcz...");
		AppendMenu(mPlik, MF_SEPARATOR, 0, "");
		AppendMenu(mPlik, MF_STRING, 101, "&Koniec");
		mInfo = CreateMenu();
		AppendMenu(mInfo, MF_STRING, 200, "&Autor...");
		mGlowne = CreateMenu();
		AppendMenu(mGlowne, MF_POPUP, (UINT_PTR) mPlik, "&Plik");
		AppendMenu(mGlowne, MF_POPUP, (UINT_PTR) mInfo, "&Informacja");
		SetMenu(okno, mGlowne);
		DrawMenuBar(okno);

	case WM_COMMAND: //reakcje na wybór opcji z menu
		switch (wParam)
		{
		case 100: if(MessageBox(okno, "Zapiszczeæ?", "Pisk", MB_YESNO) == IDYES)
					MessageBeep(0);
                  break;
		case 101: DestroyWindow(okno); //wysylamy meldunek WM_DESTROY
        		  break;
		case 200: MessageBox(okno, "Imiê i nazwisko:\nNumer indeksu: ", "Autor", MB_OK);
		}
		return 0;
	
	case WM_LBUTTONDOWN: //reakcja na lewy przycisk myszki
		{
			int x = LOWORD(lParam);
			int y = HIWORD(lParam);

			if (abs(slider.cursor.x - x) <= ROZMIAR && abs(slider.cursor.y - y) <= ROZMIAR) {
				przesuwamy_slider = true;
			}

			for (int i = 0; i < LICZBA_PUNKTOW; i++) {
				if (abs(Bezier[i].x - x) <= ROZMIAR && abs(Bezier[i].y - y) <= ROZMIAR) {
					ruszamy_krzywa = true;
					idx = i;
					break;
				}
			}

			return 0;
		}
	case WM_MOUSEMOVE: //reakcja na ruch myszki
	{
		if (przesuwamy_slider) {
			int x = LOWORD(lParam);
			int y = HIWORD(lParam);

			if (slider.pnt1.x <= x && x <= slider.pnt2.x) {
				slider.cursor.x = x;
				slider.val = float((slider.cursor.x -slider.pnt1.x)) / (slider.pnt2.x - slider.pnt1.x);
				InvalidateRect(okno, NULL, true);
			}			
		}

		if (ruszamy_krzywa) {
			int x = LOWORD(lParam);
			int y = HIWORD(lParam);

			Bezier[idx].x = x;
			Bezier[idx].y = y;
			InvalidateRect(okno, NULL, true);
		}
		return 0;
	}
	case WM_LBUTTONUP: //reakcja na ruch myszki
	{
		przesuwamy_slider = false;
		ruszamy_krzywa = false;
		return 0;
	}
	case WM_PAINT:
		{			
			PAINTSTRUCT paint;
			HDC kontekst;

			kontekst = BeginPaint(okno, &paint);

			//HBRUSH pioro = CreateSolidBrush(RGB(255, 0, 0));
			//SelectObject(kontekst, pioro);

			Rysuj_Slider(kontekst);

			//PolyBezier(kontekst, Bezier, LICZBA_PUNKTOW);
			Rysuj_Pikselowo_Beziera(kontekst);
			Rysuj_Algorytm_Beziera(&kontekst, Bezier, LICZBA_PUNKTOW);


			
			Graphics grafika(kontekst);		

			EndPaint(okno, &paint);

			return 0;
		}
  	
	case WM_DESTROY: //obowi¹zkowa obs³uga meldunku o zamkniêciu okna
		PostQuitMessage (0) ;
		return 0;
	default: //standardowa obs³uga pozosta³ych meldunków
		return DefWindowProc(okno, kod_meldunku, wParam, lParam);
	}
}
