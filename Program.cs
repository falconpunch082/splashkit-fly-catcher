using System;
using System.ComponentModel;
using System.Drawing;
using SplashKitSDK;
using static SplashKitSDK.SplashKit;

// Declaring constants
int SCREEN_WIDTH = 800;
int SCREEN_HEIGHT = 600;
int SPIDER_RADIUS = 25;
int SPIDER_SPEED = 3;
int FLY_RADIUS = 10;

// Declaring variables
int spiderX = SCREEN_WIDTH / 2;
int spiderY = SCREEN_HEIGHT / 2;
int flyX = Rnd(SCREEN_WIDTH);
int flyY = Rnd(SCREEN_HEIGHT);
bool flySpawn = false;
long appearTime = 1000 + Rnd(2000);
long escapeTime = 0;
int score = 0;

// Create fly spawn timer
CreateTimer("Timer");
StartTimer("Timer");

// Opening window
Window window = OpenWindow("Fly Catcher || Use keypads || Press ESC to quit", SCREEN_WIDTH, SCREEN_HEIGHT);

// Load sound effect
LoadSoundEffect("Gulp", "Gulp Sound Effect.mp3");

// Making while loop to shut down game if escape button pressed
while (!KeyDown(SplashKitSDK.KeyCode.EscapeKey)) {

    // Game background
    FillRectangle(ColorWhite(), 0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
    // Adding spider
    FillCircle(ColorBlack(), spiderX, spiderY, SPIDER_RADIUS);

    // Input for player spider
    if (KeyDown(SplashKitSDK.KeyCode.LeftKey)) {
        if (spiderX - SPIDER_SPEED > 0) {
            spiderX = spiderX - SPIDER_SPEED;
        }
    }
    
    if (KeyDown(SplashKitSDK.KeyCode.RightKey)) {
        if (spiderX + SPIDER_SPEED < SCREEN_WIDTH) {
            spiderX = spiderX + SPIDER_SPEED;
        }
    }
    
    if (KeyDown(SplashKitSDK.KeyCode.UpKey)) {
        if (spiderY + SPIDER_SPEED < SCREEN_HEIGHT) {
            spiderY = spiderY - SPIDER_SPEED;
        }
    }
    
    if (KeyDown(SplashKitSDK.KeyCode.DownKey)) {
        if (spiderY - SPIDER_SPEED > 0) {
            spiderY = spiderY + SPIDER_SPEED;
        }
    }
    
    // Fly Behaviour
    if (!flySpawn && TimerTicks("Timer") > appearTime) {
        flySpawn = true;
        flyX = Rnd(SCREEN_WIDTH);
        flyY = Rnd(SCREEN_HEIGHT);
        escapeTime = TimerTicks("Timer") + Rnd(2000, 7000);
    } 
    
    if (flySpawn && TimerTicks("Timer") > escapeTime) {
        flySpawn = false;
        appearTime = TimerTicks("Timer") + 1000 + Rnd(2000);
    }

    if (flySpawn) {
        FillCircle(ColorDarkGreen(), flyX, flyY, FLY_RADIUS);
        DrawText($"Flies Caught: {score}", ColorBlack(), 0, 0);
    }

    if (!flySpawn) {
        FillCircle(ColorWhite(), flyX, flyY, FLY_RADIUS);
        DrawText($"Flies Caught: {score}", ColorBlack(), 0, 0);
    }
    
    // Collision
    if (flySpawn && CirclesIntersect(spiderX, spiderY, SPIDER_RADIUS, flyX, flyY, FLY_RADIUS)) {
        flySpawn = false;
        appearTime = TimerTicks("Timer") + 1000 + Rnd(2000);
        PlaySoundEffect("Gulp");
        score++;
    }

    RefreshScreen(60);
    ProcessEvents();
}