

const SHOTS = {
	NONE : "None",
	ROCK : "Rock",
	PAPER : "Paper",
	SCISSORS : "Scissors"
};

const ROLL_RESULTS = {
	PENDING : "Pending",
	TIED : "Tied",
	WON : "Won",
	LOST : "Lost",
	OPPONENT_QUIT: "OpponentQuit"
};

/*
 *	Class to manage all pending and active matches
 */
function GameController() {

	this.currentMatchId = 0;
	this.pendingUsers = [];
	this.activeMatches = {};

	/*
	 *	Attempts to match the given user id with a pending one. Otherwise adds it to pending.
	 */
	this.match = function (userId, callback) {

		for(var i = 0; i < this.pendingUsers.length; ++i) {
			console.log(this.pendingUsers[i].userId);
		}

		if(callback == null) {
			this.log("Missing callback for match()");
			return;
		}

		if(userId == null) {
			this.log("Invalid params for match()");
			callback(null);
			return;
		}

		// Update callback if user is already pending
		var pendingUserIndex = this.getPendingUserIndex(userId);
		if(pendingUserIndex != -1) {
			this.pendingUsers[pendingUserIndex].callback = callback;
			return;
		}

		// Clean up existing match for user
		var activeMatchId = this.getActiveMatchId(userId);
		if(activeMatchId != null) {
			delete this.activeMatches[activeMatchId];
		}

		// Match with first pending user
		var pendingUser = this.popFirstPendingUser();
		if(pendingUser != null) {

			var matchId = 'm' + this.currentMatchId++;
			this.activeMatches[matchId] = new Match(matchId, pendingUser.userId, userId); 
			pendingUser.callback(matchId);
			callback(matchId);

		} else {
			this.pendingUsers.push(new PendingUser(userId, callback));
		}
	}

	/*
	 *	Attempts to resolve a roll for the given match.
	 */
	this.roll = function (matchId, userId, shot, callback) {

		if(callback == null) {
			this.log("Missing callback for this()");
			return;
		}

		if(matchId == null || userId == null ||  shot == null) {
			this.log("Invalid params for roll()");
			callback(null);
			return;
		}

		// This match no longer exists
		if(this.activeMatches[matchId] === undefined) {
			callback({
				"result": ROLL_RESULTS.OPPONENT_QUIT
			});
			return;
		}

		var match = this.activeMatches[matchId];

		// Peform the roll
		var roll = match.tryRoll(userId, shot, callback);

		// If we aren't pending, we can send the results back to both users.
		if(roll.result != ROLL_RESULTS.PENDING) {

			var theirUserId = match.getOtherUserId(userId);
			var pendingUserShot = roll[theirUserId];
			
			// Determine the contextual results for each user
			var theirResult = roll.result;
			var myResult = roll.result;		
			if(roll.result == userId) {
				theirResult = ROLL_RESULTS.LOST;
				myResult = ROLL_RESULTS.WON;
			} else if(roll.result == theirUserId) {
				theirResult = ROLL_RESULTS.WON;
				myResult = ROLL_RESULTS.LOST;
			}

			match.pendingRollCallback({
				"myShot" : pendingUserShot,
				"theirShot" : shot,
				"result" : theirResult
			});
			match.pendingRollCallback = null;

			callback({
				"myShot" : shot,
				"theirShot" : pendingUserShot,
				"result" : myResult
			});
		}
	}
	
	/*
	 *	Cleans up any pending or active matches for the given user.
	 */
	this.quit = function (userId) {
		if(userId == null) {
			return;
		}

		// Let the waiting user know that this user has quit
		var activeMatchId = this.getActiveMatchId(userId);
		if(activeMatchId != null) {
			var match = this.activeMatches[activeMatchId];
			if(match.pendingRollCallback != null) {
				match.pendingRollCallback({
					"result": ROLL_RESULTS.OPPONENT_QUIT
				});
				match.pendingRollCallback = null;
			}
		}

		this.removeUserEntries(userId);
	}

	/*
	 *	Removes any pending or active matches for the given user
	 */
	this.removeUserEntries = function (userId) {

		var pendingUserIndex = this.getPendingUserIndex(userId);
		if(pendingUserIndex != -1) {
			this.pendingUsers.splice(pendingUserIndex, 1);
		}

		var activeMatchId = this.getActiveMatchId(userId);
		if(activeMatchId != null) {
			delete this.activeMatches[activeMatchId];
		}
	}

	/*
	 *	Removes and returns the first pending user.
	 */
	this.popFirstPendingUser = function () {

		if(this.pendingUsers.length == 0) {
			return null;
		}

		var pendingUser = this.pendingUsers.shift();
		if(pendingUser == undefined) {
			return null;
		}
		return pendingUser;		
	}

	/*
	 *	Returns the index of the pending user. If user is not pending returns -1.
	 */
	this.getPendingUserIndex = function (userId) {

		for(var i = 0; i < this.pendingUsers.length; ++i) {
			if (this.pendingUsers[i].userId == userId) {
				return i;
			}
		}
		return -1;
	}

	/*
	 *	Returns the match id of the given user. If the user is not in a match returns null.
	 */
	this.getActiveMatchId = function (userId) {
		for(var matchId in this.activeMatches) {
			if(this.activeMatches[matchId].userIdA === userId || this.activeMatches[matchId].userIdB === userId) {
				return matchId;
			}
		}
		return null;
	}

	/*
	 *	Simple log utility
	 */
	this.log = function(value) {
		console.log('gamecontroller - ' + value);
	}

	/*
	 *	Class for storing pending user data.
	 */
	function PendingUser (userId, callback) {

		this.userId = userId;
		this.callback = callback;
	}

	/*
	 *	Class for managing a match between two users
	 */
	function Match (matchId, userIdA, userIdB) {

		this.matchId = matchId;
		this.userIdA = userIdA;
		this.userIdB = userIdB;
		this.rolls = [];
		this.pendingRollCallback = null;

		/*
		 *	Tries to perform the roll. If both users have shot, will return the results.
		 */
		this.tryRoll = function(userId, shot, callback) {

			var currentRoll = this.rolls[this.rolls.length - 1];
			
			// Prevent reroll
			if(currentRoll[userId] != SHOTS.NONE) {
				return currentRoll;
			}
			currentRoll[userId] = shot;
			var shotA = currentRoll[this.userIdA];
			var shotB = currentRoll[this.userIdB];

			// Still waiting on one user
			if(shotA == SHOTS.NONE || shotB == SHOTS.NONE) {
				this.pendingRollCallback = callback;
				return currentRoll;
			} 

			// Tie
			else if (shotA == shotB) {
				currentRoll.result = ROLL_RESULTS.TIED;
			} 

			// Must be a winner
			else {
				if (shotA == SHOTS.ROCK) {
					currentRoll.result = shotB == SHOTS.PAPER ? this.userIdB : this.userIdA;
				} else if (shotA == SHOTS.PAPER) {
					currentRoll.result = shotB == SHOTS.ROCK ? this.userIdA : this.userIdB;
				} else {
					currentRoll.result = shotB == SHOTS.PAPER ? this.userIdA : this.userIdB;
				}				
			}

			// Add our next roll since this one is complete
			this.addRoll();

			return currentRoll;
		}

		/*
		 *	Adds an empty roll object to the rolls array.
		 */
		this.addRoll = function() {
			var roll = { };
			roll[this.userIdA] = SHOTS.NONE;
			roll[this.userIdB] = SHOTS.NONE;
			roll['result'] = ROLL_RESULTS.PENDING;
			this.rolls.push(roll);
		}

		/*
		 *	Returns the other user id.
		 */
		this.getOtherUserId = function(userId) {
			return userId == this.userIdA ? this.userIdB : this.userIdA;
		}

		this.addRoll();
	}	
}

module.exports = {
	'GameController' : GameController,
	'SHOTS' : SHOTS,
	'ROLL_RESULTS' : ROLL_RESULTS,
}