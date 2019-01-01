var WebSocketServer = require('websocket').server;
var osc = require('node-osc');

var http = require('http');
var port = 50013
var udpPort = 9998

//create osc client
var client = new osc.Client('127.0.0.1', udpPort);

//create websocket server
var server = http.createServer(function(request, response) {
	// process HTTP request. Since we're writing just WebSockets
	// server we don't have to implement anything.
});
server.listen(port, function() { console.log('listening to port ' + port);});

wsServer = new WebSocketServer({
	httpServer: server
});



wsServer.on('request', function(request) {
	var connection = request.accept(null, request.origin);
	console.log('Client connected');
	// This is the most important callback for us, we'll handle
	// all messages from users here.
  
	connection.on('message', function(message) {
		if (message.type === 'utf8') {
			// process WebSocket message
			//sendOSC(parseFloat(message.utf8Data));
			console.log(message.utf8Data);
		}
	});

	connection.on('close', function(connection) {
		// close user connection
		console.log('Client disconnected')
	});
});


function sendOSC(message) {
	client.send('/oscAddress', message, function () {
		//client.kill();
	});
}
