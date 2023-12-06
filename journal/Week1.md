# Week 1 journey
1. [`Day 1`](#day-1)
2. [`Day 2`](#day-2)
3. [`Day 3`](#day-2)

## Day 1
> [!IMPORTANT]
> I needed to start!!
#### I made sure to do research:
The first step to any project. The research. I went ahead to YouTube to see how other people have created .exe files and the most common way was through `Visual Studio`. Now I was fimiliar with `Visual Studio Code` which at the time, I thought was the same thing. So after 10 minutes of browsing the settings of Visual Studio Code, I did something no one ever does...watches the entire 1st minute of the Youtube turtorial :sweat_smile:. I then installed VS with all the dependancies and so on. Created a new project and had my ReadMe file sorted. Was looking good.

## Day 2
> [!NOTE]
> I have to get coding done.
#### I wanted at least one part of my project working
First things first...get access to the twitch chat to read the commands/song requests. Although I had an idea on how to do this, I still proceeded to do research and find more ways to complete this section.
- Web scrape the chat
  - This is an issue
    - May break rules on Twitch
    - May not be accurate
    - Scrapping wasn't really my strong suite
- Use an OAuth key
  - This may work actually
    - Just go get the auth key and paste it in the code
    - Easy to work around an auth key incase any perms are needed throughout project
  - Not user friendly
    - Not everyone is a developer :unamused:
    - May leak OAuth keys due to human error
- ChatBot for music control
  - This is a great idea
    - Will allow anyone to use it, its a bot
    - people just connect the bot to the chat
    - gets the job done on the cloud as well
  - Defeats the whole purpose of creating a .exe file

The options were great. Unfortunatley we couldn't use the most user friendly chat bot method, so instead we go with the OAuth method to avoid rule violating.
While looking at how to do this, I found a [video](https://www.youtube.com/watch?v=Ufgq6_QhVKw) by [HonestDanGames](https://www.twitch.tv/honestdangames) who is a game developer and streamer on twitch. Popped into his stream and he said it was cool if I used some of his code. So with that guidance I managed to complete the OAuth section of the code

## Day 3
> [!IMPORTANT]
> Unfinished work!
#### I needed to complete my day 3 goals today
Sacrifices have to be made today. I had initially planned to take day 4 off and come back to this project on Day 5, but with my programme having build errors along with the login page links not responding, I need to code and fix that today. Day 4's work will have to be done on day 5. This wasn't an easy task as I had a few things happening today as well, so along with getting those done I needed to complete the project.
- First things first. Understand the errors that are given to me
  - I didn't know what these errors were as they diffrent from build errors I was recieving in Java
  - I went ahead to do research on what each error meant. I had 2 major ones
    - My 'assembly' code was duplicating files.
    - I had .net packages that didn't match the version of the lib package installed
  - A few other errors popped up but they were domino affect errors
- Second thing to do was find solutions.
  - I couldn't find any youtube videos that fixed this error
  - I then resorted to trial and error which took me the whole day but it was worth it

At the end of Day 3, I had my programme form finally pop up, the connect button ran it's code (which then later crashed) but the programme was able to build, I understood the structure of my project and it's files better, and I now know not to duplicate ```[assembly: <text>]``` with the same text accross my project.
I am loving this project so far and the challenges it is presenting me, but dammm bro...the link isn't working??? :confused:
