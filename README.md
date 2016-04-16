# Cortana Deep Dive
This simple demo was in response to [a question on Quora.com](https://www.quora.com/On-Windows-10-PC-Is-there-a-way-to-ask-or-otherwise-direct-Cortana-to-read-from-a-text-document-or-other-similar-file/answer/Kelvin-McDaniel) where the OP wanted to know if Cortana could open and read an arbitrary file on the user's device.


# SPECIAL THANKS
To the folks at Orlando Code Camp 2016 who sat through the infamous 24 minutes of technical difficulties where the facility's projector refused to work... I appreciate your patience. Hopefully this repository will make up a bit for the waste of your time.  :)


# Requirements:
1. **Before you build**: Using the solution Configuration Manager, verify that **Build** and **Deploy** are checked for the .UWP project.
2. You'll need to **Build** and **Deploy** the app to whatever target you want to run it on (Desktop, Tablet, Mobile, etc).
3. Of course Cortana will need to be enabled and turned on for it to work.  :)
4. For Desktop/Laptop/Tablet: Create a folder called **Cortana** on your **D: drive** (D:\Cortana).
5. For Mobile: make sure an SD Card is present and create a folder on the root of the card called **Cortana**. The path should resolve to **D:\Cortana**.
6. In the **Cortana** folder (that you created in 4 or 5) add a text file called **test.txt**.
7. In the **test.txt** file, add some content... but no more than ** *256 characters, total* ** (includding punctuation and spaces -- I know, that sucks, but that's Microsoft's doing, not mine).
8. Make sure you save the file after you've added your content.
9. **You need to run the app at least once to make sure that the voice command definition file loaded.**
10. Wait three to five seconds to make sure the load completes... but then you can exit the app.

Once you've done that you should be good to go!

Ask Cortana to ** *read out loud using your extensions* **. 

See if you can figure out how to use the other phrase that works too. 
(Hint: it's in the **CortanaDeepDive.UWP/VoiceCommandDefinitions/General.xml** file)

# -- Happy Coding!
