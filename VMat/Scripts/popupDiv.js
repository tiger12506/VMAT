/* -----------------------------------------------
Floating layer - v.1
(c) 2006 www.haan.net
contact: jeroen@haan.net
You may use this script but please leave the credits on top intact.
Please inform us of any improvements made.
When usefull we will add your credits.
------------------------------------------------ */

x = 20;
y = 70;

function setVisible(obj) {
    var thisObj = document.getElementById(obj);

    thisObj.style.visibility = 'visible';
    centerHorizontally(obj);
}

function closeWindow(obj) {
    var thisObj = document.getElementById(obj);

    thisObj.style.visibility = 'hidden';
}

function placeIt(obj) {
    obj = document.getElementById(obj);
    if (document.documentElement) {
        theLeft = document.documentElement.scrollLeft;
        theTop = document.documentElement.scrollTop;
    }
    else if (document.body) {
        theLeft = document.body.scrollLeft;
        theTop = document.body.scrollTop;
    }
    theLeft += x;
    theTop += y;
    obj.style.left = theLeft + 'px';
    obj.style.top = theTop + 'px';
    setTimeout("placeIt('layer1')", 500);
}

function centerHorizontally(objectID) {
    var thisObj = document.getElementById(objectID);
    var width = (window.innerWidth ? window.innerWidth : document.body.clientWidth);
    var objectWidth = parseInt(thisObj.style.width);
    var newLocation = (width - objectWidth) / 2;
    thisObj.style.left = newLocation + 'px';
}
