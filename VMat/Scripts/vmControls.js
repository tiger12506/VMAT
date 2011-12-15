function toggleMachineDetails(obj) {
    var thisObj = document.getElementById(obj);
    thisObj.style.display = ((thisObj.style.display == 'block') ? 'none' : 'block');
}

function toggleMachineStatus(obj) {
    var machineName = obj.slice(obj.indexOf("-") + 1);
    var objImage = document.getElementById(obj);
    //var image = (objImage.style.src === "/Images/icon_led-green.png

    //objImage.style.src = 

    var status = ToggleMachineName(machineName);
    //objImage.style.src = (status === true) ? "/Images/icon_led-green.png" : 
}

function getStatusIcon(obj) {
    if (IsMachineRunning(machineName)) {
        alert("Running");
        return "<img id=" + obj + "src='/Images/icon_led-green.png' alt='?' />";
    } else {
        alert("Not running");
        return "<img id=" + obj + "src='/Images/icon_led-red.png' alt='?' />";
    }
}
