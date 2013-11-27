backuper
========

This is a simple console app in C# for backing up a directory as specified in the app.config file.

I use it to backup my Quicken files to a folder on Dropbox.

Couldn't I have just done this with a .bat script? Yep. But I didn't, so deal with it. 

Ultimately, I would like this app to be a replacement for a Carbonite-type backup solution, beacause I have gotten weary of paying for these types of services. They still don't do everything that I want for the money that I am willing to pay.

Usage
=====
Change the app.config file settings to match your source and destination directories:
```
<add key="sourceFolder" value="YourSourceFolderGoesHere"/>
<add key="detinationFolder" value="YourDestinationFolderGoesHere"/>
```
Compile the source to an executable and copy the .exe and .config files from the project bin folder.

I find it useful to setup a Windows scheduled task to run this .exe every night.

Future Plans
============
- Read-in source and destination directories from a database or xml file, so that multiple directories can be backed-up in one run.
- Compress files into a timestamped Zip archive, to enable storing multiple versions of files in case of source corrunption.
- I also like the idea of using Azure storage or Amazon S3 as the backup destination, rather than a traditional file system.


