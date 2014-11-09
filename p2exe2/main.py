#Define the imports
from keypresser import Keypresser
from twitch import Twitch
t = Twitch()
k = Keypresser()
 
#Enter your twitch username and oauth-key below, and the app connects to twitch with the details.
#Your oauth-key can be generated at http://twitchapps.com/tmi/
username = "jlchamaa"
key = "oauth:w7kjeztre4ypvzt2dx40r2a8x8aqcl"
t.twitch_connect(username, key);
 
#The main loop
while True:
    #Check for new mesasages
    new_messages = t.twitch_recieve_messages();

    if not new_messages:
        #No new messages...
        continue
    else:
        for message in new_messages:
            # we got a message. Let's extract some details from it
            msg = message['message'].lower()
            username = message['username'].lower()
            print(username + ": " + msg);
 
            #This is where you change the keys that shall be pressed and listened to.
            #The code below will simulate the key q if "q" is typed into twitch by someone
            #.. the same thing with "w"
            #Change this to make Twitch fit to your game!
            if msg=="0": k.key_press("0");
            if msg=="1": k.key_press("1");
            if msg=="r": k.key_press("r");
            
            
            
            