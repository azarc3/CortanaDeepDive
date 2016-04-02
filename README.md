# CortanaDeepDive
The content for presentation at Code Camp in 2016.

This simple demo was in response to a question on Quora.com (https://www.quora.com/On-Windows-10-PC-Is-there-a-way-to-ask-or-otherwise-direct-Cortana-to-read-from-a-text-document-or-other-similar-file/answer/Kelvin-McDaniel) where the OP wanted to know if Cortana could open and read an arbitrary file on the user's device.

# Requirements:
1. You'll need to Build and Deploy the app to whatever target you want to run it on (Windows Desktop/Laptop/Tablet/Mobile, etc).
2. Of course Cortana will need to be enabled and turned on for it to work.  :)
3a. For Desktop/Laptop/Tablet: Create a folder called "Cortana" (without the quotes) in your Documents library. File Explorer > Documents > Cortana
3b. For Mobile: make sure an SD Card is present and create a folder on the root of the card called "Cortana" (without the quotes). The path *should* resolve to D:\Cortana .
4. In the "Cortana" folder you created in 3a or 3b, add a text file called "test.txt".
5. In the "test.txt" file, add some content... but no more than *256 characters, total* (includding punctuation and spaces -- I know, that sucks, but that's Microsoft's doing, not mine).
6. Make sure you save the file after you've added your content.
