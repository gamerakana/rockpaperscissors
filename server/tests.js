
const gameControllerSource = require("./gamecontroller");
var gameController = new gameControllerSource.GameController();
const SHOTS = gameControllerSource.SHOTS;
const ROLL_RESULTS = gameControllerSource.ROLL_RESULTS;

function assert(a, b) {
	if(a !== b) {
		throw new Error('Assertion failed ' + a + ' is not equal to ' + b);
	}
}

function testMatchSuccess () {
	gameController.match('userA', function (matchId) {
		assert(matchId, 'm0');
	});

	gameController.match('userB', function (matchId) {
		assert(matchId, 'm0');
	});	
}

function testMatchFailure_InvalidUserId () {
	gameController.match(null, function (matchId) {
		assert(matchId, null);
	});
}

function testMatchFailure_InvalidCallback () {
	gameController.match('userA', null);
}

function testRollTie () {

	var returnedMatchId;
	gameController.match('userA', function (matchId) {
		returnedMatchId = matchId;
	});

	gameController.match('userB', function (matchId) { });	

	gameController.roll(returnedMatchId, 'userA', SHOTS.ROCK, function (result) {
		assert(result.result, ROLL_RESULTS.TIED);
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.ROCK, function (result) { });


	gameController.roll(returnedMatchId, 'userA', SHOTS.PAPER, function (result) {
		assert(result.result, ROLL_RESULTS.TIED);
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.PAPER, function (result) { });


	gameController.roll(returnedMatchId, 'userA', SHOTS.SCISSORS, function (result) {
		assert(result.result, ROLL_RESULTS.TIED);
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.SCISSORS, function (result) { });
}

function testRollAWins () {
	
	var returnedMatchId;
	gameController.match('userA', function (matchId) {
		returnedMatchId = matchId;
	});

	gameController.match('userB', function (matchId) { });	

	gameController.roll(returnedMatchId, 'userA', SHOTS.ROCK, function (result) {
		assert(result.result, 'Won');
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.SCISSORS, function (result) {
		assert(result.result, 'Lost');
	});


	gameController.roll(returnedMatchId, 'userA', SHOTS.PAPER, function (result) {
		assert(result.result, 'Won');
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.ROCK, function (result) {
		assert(result.result, 'Lost');
	});


	gameController.roll(returnedMatchId, 'userA', SHOTS.SCISSORS, function (result) {
		assert(result.result, 'Won');
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.PAPER, function (result) { });	

}

function testRollBWins () {

	var returnedMatchId;
	gameController.match('userA', function (matchId) {
		returnedMatchId = matchId;
	});

	gameController.match('userB', function (matchId) { });	

	gameController.roll(returnedMatchId, 'userA', SHOTS.ROCK, function (result) {
		assert(result.result, 'Lost');
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.PAPER, function (result) {
		assert(result.result, 'Won');
	});


	gameController.roll(returnedMatchId, 'userA', SHOTS.PAPER, function (result) {
		assert(result.result, 'Lost');
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.SCISSORS, function (result) {
		assert(result.result, 'Won');
	});


	gameController.roll(returnedMatchId, 'userA', SHOTS.SCISSORS, function (result) {
		assert(result.result, 'Lost');
	});
	gameController.roll(returnedMatchId, 'userB', SHOTS.ROCK, function (result) {
		assert(result.result, 'Won');
	});
}

function log (output) {
	console.log("GameControllerTests - " + output);
}

var testsRun = 0;
var testsPassed = 0;

function runTest(test) {
	++testsRun;
	try {
		test();
		++testsPassed;
		log(test.name + ' succeeded');
	} catch (error) {
		log(test.name + ' failed. ' + error.message);
	}
}

runTest(testMatchSuccess);
runTest(testMatchFailure_InvalidUserId);
runTest(testMatchFailure_InvalidCallback);
runTest(testRollTie);
runTest(testRollAWins);
runTest(testRollBWins);

log('Testing complete ' + testsPassed + ' of ' + testsRun + ' passed.');