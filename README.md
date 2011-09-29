# VMAT

To run the program from _Command Prompt_ (can not run it from a Unix shell), use the command:

	cd "c:\Program Files (x86)\Common Files\microsoft shared\DevServer\10.0"
	start /B WebDev.WebServer40.exe /port:8080 /path:"c:\WebsiteDirectory"

Replace "WebsiteDirectory" with the path to the root of your local webpage files. The value for the /port option can be any valid, unused port.

Navigate in your web browser to _http://localhost:8080/_, replacing "8080" with the port number you launched the local server on.