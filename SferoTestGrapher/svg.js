var choicesNumber = 2;
var actualChoice = 0;



window.onload= function(){
    const filepicker = document.getElementById("filepicker");

    filepicker.addEventListener("change", (event) => {

        actualChoice=0
        var first = true;
        const files = event.target.files;
        MainImageContainer.innerHTML="";
		choicesNumber=files.length/2;
        for(var i=0;i<files.length;i+=2){
        //for (const file of files) {
            var path = files[i];
            var svg = files[i+1];            
            var relativePathSvg =  `${svg.webkitRelativePath}`;                                                                        

            StartFileReader(path, relativePathSvg,first);
            first=false;
        }        
    });
}

function StartFileReader(path,relativePathSvg,first){
    const reader = new FileReader();
    //parse file with position and velocities
    reader.addEventListener(
        "load",
        () => {
            var wholeText = reader.result;

            var trackData = JSON.parse(wholeText);

            //split each line
            var lines = wholeText.split('\n');

            var roomName = trackData.roomName;
            var roomSize = trackData.roomDimensions;            
            var pointsArray=trackData.records;

            pointsArray.forEach(point =>{
                point['pos']['x']=parseFloat(point['pos']['x']).toFixed(2);
                point['pos']['y']=parseFloat(point['pos']['y']).toFixed(2);
                point['vel']['x']=parseFloat(point['vel']['x']).toFixed(2);
                point['vel']['y']=parseFloat(point['vel']['y']).toFixed(2);
            });
            

            CreatePathCointaner(roomName, roomSize, relativePathSvg, pointsArray,first);        
        },
        false,
    );
    reader.readAsText(path);
}





function CreatePathCointaner(roomNameStr, roomSizeStr, svgSrc, pointsArray,shown){    
    var pathContainer = document.createElement("div");
    pathContainer.className="PathContainer";
    pathContainer.style.display=shown?"block":"none";
    MainImageContainer.appendChild(pathContainer);

    var roomName = document.createElement('div');
    roomName.innerHTML = roomNameStr;
    roomName.className="Title";
    pathContainer.appendChild(roomName);

    var roomSize = document.createElement('div');
    roomSize.innerHTML = roomSizeStr;
    roomSize.className="Title";
    pathContainer.appendChild(roomSize);

    var image = document.createElement('img');
    image.src = svgSrc;
    image.className="Path";
    pathContainer.appendChild(image);

    var pointsContainer = document.createElement("div");
    pointsContainer.className="PointsContainer";    
    pathContainer.appendChild(pointsContainer);

    pointsArray.forEach(pointPos => {
        pointNode = document.createElement("div");
        pointNode.className="Point";
        pointsContainer.appendChild(pointNode);
        
        var width = getComputedStyle(document.body).getPropertyValue('--pathWidth').replace('px','');
        width=parseInt(width);
        var height = width*8/5;

        pointNode.style.left = pointPos['pos']['x']/40 * width + width/2 -5 +"px";
        pointNode.style.bottom = pointPos['pos']['y']/64 * height -5 +"px";
        
        pointNode.style.setProperty('--posText','"'+"posX: "+pointPos['pos']['x']+", posY: "+pointPos['pos']['y']+'"');
        pointNode.style.setProperty('--velText','"'+"velX: "+pointPos['vel']['x']+", velY: "+pointPos['vel']['y']+'"');
        //style it correctly
    });        
}

function Next(){
    if(choicesNumber > 0){        
        MainImageContainer.children[actualChoice++].style.display="none";
        if(actualChoice == choicesNumber)actualChoice=0;
        MainImageContainer.children[actualChoice].style.display= "block";
    }
}

function Prev(){
    if(choicesNumber > 0){        
        MainImageContainer.children[actualChoice--].style.display="none";
        if(actualChoice == -1)actualChoice=choicesNumber-1;
        MainImageContainer.children[actualChoice].style.display= "block";
    }
}

