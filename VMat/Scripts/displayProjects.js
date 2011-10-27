﻿function loadXMLDoc(dname) {
    if (window.XMLHttpRequest) {
        xhttp = new XMLHttpRequest();
    }
    else {
        xhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xhttp.open("GET", dname, false);
    xhttp.send("");
    return xhttp.responseXML;
}

function displayList() {
    xml = loadXMLDoc("Projects.xml");
    xsl = loadXMLDoc("Projects.xsl");

    // code for IE
    if (window.ActiveXObject) {
        ex = xml.transformNode(xsl);
        document.getElementById("display-projects-list").innerHTML = ex;
    }
    // code for Mozilla, Firefox, Opera, etc.
    else if (document.implementation && document.implementation.createDocument) {
        xsltProcessor = new XSLTProcessor();
        xsltProcessor.importStylesheet(xsl);
        resultDocument = xsltProcessor.transformToFragment(xml, document);
        document.getElementById("display-projects-list").appendChild(resultDocument);
    }
}