bucket-cleaner
==============

Delete objects from AWS S3 bucket that are older than a specified age

#Tested

mono 3.10.0

#Usage 

    export AWS_ACCESS_KEY=x 
    export AWS_SECRET_KEY=y
    mono bucket-cleaner.exe <bucket> <max_age_in_hours>


