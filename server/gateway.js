
/*
 *	The main gateway for RPS. Routes and handles all server calls.
 */
const express = require("express");
const app = express();
const bodyParser = require('body-parser');
const port = 3000;
const gameControllerSource = require("./gamecontroller");
var gameController = new gameControllerSource.GameController();
var ROLL_RESULTS = gameControllerSource.ROLL_RESULTS;


app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended:true}));

/*
 *	Match end point. Attempts to match the given user up with a pending one.
 */
app.post('/match', function(request, response) {	
	
	if(request.body.userId == undefined) {
		response.status(400).send("Invalid Input");
	} else {

		gameController.match(request.body.userId, function(matchId) {
			if(matchId == null) {
				response.status(500).send("Unknown failure");
			} else {
				response.send({"matchId":matchId});
			}
		});
	}
});

/*
 *	Roll end point. Handles each user's shot and returns the result of the roll.
 */
app.post('/roll', function(request, response) {
	
	if(request.body.matchId == undefined || 
		request.body.userId == undefined ||
		request.body.shot == undefined) {
			response.status(400).send("Invalid Input");
		} else {
			
			gameController.roll(request.body.matchId, request.body.userId, request.body.shot, function(result) {
				if(result == null) {
					response.status(500).send("Unknown failure");
				} else if (result.result == ROLL_RESULTS.OPPONENT_QUIT) {
					response.status(999).send(result.result);
				} else {
					response.send(result);
				}
			});
		}
});

/*
 *	Quit end point. Cleans up any left over data for the given user.
 */
app.post('/quit', function(request, response) {
	
	if(request.body.userId == undefined) {
		response.status(400).send("Invalid Input");
	} else {
		gameController.quit(request.body.userId);
		response.send("OK");
	}
});

app.listen(port, function() {
	console.log('RPS gateway listening on port ' + port);
});
