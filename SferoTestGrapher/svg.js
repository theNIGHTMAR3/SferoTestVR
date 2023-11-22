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
            var records=trackData.records;

            records.forEach(point =>{
                point['pos']['x']=parseFloat(point['pos']['x']).toFixed(2);
                point['pos']['y']=parseFloat(point['pos']['y']).toFixed(2);
                point['vel']['x']=parseFloat(point['vel']['x']).toFixed(2);
                point['vel']['y']=parseFloat(point['vel']['y']).toFixed(2);
            });
            

            CreatePathCointaner(roomName, roomSize, relativePathSvg, records,first);        
        },
        false,
    );
    reader.readAsText(path);
}





function CreatePathCointaner(roomNameStr, roomSizeStr, svgSrc, records,shown){    
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

    records.forEach(record => {
        pointNode = document.createElement("div");
        pointNode.className="Point";
        pointsContainer.appendChild(pointNode);
        
        var width = getComputedStyle(document.body).getPropertyValue('--pathWidth').replace('px','');
        width=parseInt(width);
        var height = width*8/5;

        pointNode.style.left = record['pos']['x']/40 * width + width/2 -5 +"px";
        pointNode.style.bottom = record['pos']['y']/64 * height -5 +"px";
        
        pointNode.style.setProperty('--posText','"'+"posX: "+record['pos']['x']+", posY: "+record['pos']['y']+'"');
        pointNode.style.setProperty('--velText','"'+"velX: "+record['vel']['x']+", velY: "+record['vel']['y']+'"');
        //style it correctly



        var div = document.createElement('div');
        div.className="List";
        div.innerHTML=`        
            <div>Data</div>
            <ul>
                <div class="List">
                    <div>pos</div>
                    <ul>
                        <li>x:`+record.pos.x+`</li>
                        <li>y:`+record.pos.y+`</li>
                    </ul>
                </div>

                <div class="List">
                    <div>vel</div>
                    <ul>
                        <li>x:`+record.vel.x+`</li>
                        <li>y:`+record.vel.y+`</li>
                    </ul>
                </div>

                <div class="List">
                    <div>sphereRecord</div>
                    <ul>
                        <li>timestamp:`+record.sphereRecord.timestamp+`</li>
                        <li>velocity:`+record.sphereRecord.velocity+`</li>
                        <li>direction:`+record.sphereRecord.direction+`</li>
                        <li>velocityVectorX:`+record.sphereRecord.velocityVectorX+`</li>
                        <li>velocityVectorY:`+record.sphereRecord.velocityVectorY+`</li>
                        <li>velocityVectorZ:`+record.sphereRecord.velocityVectorZ+`</li>
                    </ul>
                </div>

                <div class="List">
                    <div>motorRecords</div>
                    <ul>
                        <div class="List">
                            <div>motor1</div>
                            <ul>
                                <li>motorCurrent:`+record.motorRecords.motor1.motorCurrent+`A</li>
                                <li>motorVoltage:`+record.motorRecords.motor1.motorVoltage+`V</li>
                                <li>motorVelocity:`+record.motorRecords.motor1.motorVelocity+`</li>
                            </ul>
                        </div>
                        
                        <div class="List">
                            <div>motor2</div>
                            <ul>
                            <li>motorCurrent:`+record.motorRecords.motor2.motorCurrent+`A</li>
                            <li>motorVoltage:`+record.motorRecords.motor2.motorVoltage+`V</li>
                            <li>motorVelocity:`+record.motorRecords.motor2.motorVelocity+`</li>
                            </ul>
                        </div>

                        <div class="List">
                            <div>motor3</div>
                            <ul>
                                <li>motorCurrent:`+record.motorRecords.motor3.motorCurrent+`A</li>
                                <li>motorVoltage:`+record.motorRecords.motor3.motorVoltage+`V</li>
                                <li>motorVelocity:`+record.motorRecords.motor3.motorVelocity+`</li>
                            </ul>
                        </div>

                        <div class="List">
                            <div>motor4</div>
                            <ul>
                                <li>motorCurrent:`+record.motorRecords.motor4.motorCurrent+`A</li>
                                <li>motorVoltage:`+record.motorRecords.motor4.motorVoltage+`V</li>
                                <li>motorVelocity:`+record.motorRecords.motor4.motorVelocity+`</li>
                            </ul>
                        </div>
                    </ul>
                </div>
            </ul>        
        `        


        pointNode.appendChild(div);

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

