# Week 1 journey
#### Table of Contents
1. [`Day 1`](#day-1)
2. [`Day 2`](#day-2)

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
