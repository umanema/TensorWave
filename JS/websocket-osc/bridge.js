var WebSocketServer = require('websocket').server;
var osc = require('node-osc');

var http = require('http');
var port = 50013

var server = http.createServer(function(request, response) {
  // process HTTP request. Since we're writing just WebSockets
  // server we don't have to implement anything.
});
server.listen(port, function() { console.log('listening to port ' + port);});

// create the server
wsServer = new WebSocketServer({
  httpServer: server
});

// WebSocket server
wsServer.on('request', function(request) {
  var connection = request.accept(null, request.origin);
  console.log('connection');
  // This is the most important callback for us, we'll handle
  // all messages from users here.
  
  connection.on('message', function(message) {
    if (message.type === 'utf8') {
      // process WebSocket message
	  console.log(message);
    }
  });

  connection.on('close', function(connection) {
    // close user connection
  });
});

var client = new osc.Client('127.0.0.1', 3333);

client.send('/oscAddress', 200, function () {
  client.kill();
});