=========================================================================
Description

Ultimate Rock Paper Scissors is a simple rock paper scissors game made to be played solo or on a network with a friend.

=========================================================================
Getting Started

Prerequisites:
	- Unity 2017.3.0f3 or higher
	- Node.js

=========================================================================
Running the application

Server:

	Command line (Requires Node.js):
		- node 'server/gateway.js'

Client:

	- Open 'client' in the Unity editor
	- Open 'client/Assets/Scenes/Game.unity'

=========================================================================
Running server tests

The server tests require Node.js

- node server/tests.js

=========================================================================
Built With

Server:
	- Node.js
	- Express
	- body-parser

Client:
	- Unity 2017.3.0f3

=========================================================================
Implementation details

Server:

Brief:
I chose to implement the server with Node.js for its ease of use and familiarity. The server architecture uses Object Oriented design, the main component being the GameController class defined in gamecontroller.js. It wraps all data and functionality for multiplayer RPS. The gateway is simply the intermediary between client calls and the GameController class.

Client:

All client flows are constructed via a single FSM instance (GameStateController.cs). This is also the entry point for the application. Each state in this FSM represents a view controller for the MVC pattern. However, this is a bit simplified since most game states don't require a model and are simply represented with a view controller and view. This architecture is what makes this client highly extensible, as all game states are completely self contained. 

	Important files:
		- 'Assets/Scripts/Game/GameStates/RockPaperScissors/'
		- 'Assets/Scripts/Game/GameStateController.cs'

=========================================================================
What would I change?

- Make it prettier!
- Add server authentication
- Add client persistence (When entering multiplayer, each user gets a new identifier)
- Add a network layer so that the multiplayer model isn't doing that work
- Add client unit tests
- Add transitions between game states
- Add user personalization (Let them set their name)

=========================================================================
Authors

Joshua Akana
joshua.k.akana@gmail.com

=========================================================================
