// Display a 'div' component within the page like a popup window, centered horizontally in the window.

function setVisible(obj) {
    var thisObj = document.getElementById(obj);

    thisObj.style.visibility = 'visible';
    thisObj.style.width = '300px';
    thisObj.style.left = Math.round((document.documentElement.clientWidth - parseInt(thisObj.style.width)) / 2) + "px";
}

function closeWindow(obj) {
    var thisObj = document.getElementById(obj);
    thisObj.style.visibility = 'hidden';
}

function toggleMachineDetails(obj) {
    var thisObj = document.getElementById(obj);
    thisObj.style.display = ((thisObj.style.display == 'block') ? 'none' : 'block');
}