# Amazon S3 (or compatible) Setup

<!-- MarkdownTOC -->

- [Non Amazon, S3 Compatible services](#non-amazon-s3-compatible-services)
- [Setup for Cloudflare R2](#setup-for-cloudflare-r2)
- [Setup for Backblaze B2](#setup-for-backblaze-b2)
- [Standard Amazon s3 setup](#standard-amazon-s3-setup)
	- [Generate an access key user and get users ARN:](#generate-an-access-key-user-and-get-users-arn)
	- [Add the users permissions to a new \(or existing\) S3 storage bucket](#add-the-users-permissions-to-a-new-or-existing-s3-storage-bucket)

<!-- /MarkdownTOC -->


## Non Amazon, S3 Compatible services
For Amazon S3 compatible storage services you will need to see that company's documentation for how to create a storage bucket and setup an access key.   A few examples are below. NOTE: Not all providers support ACL/checksums so you may need to disable these features in the keepass options to use it. For a partial list of S3 compatible providers see: https://rclone.org/s3/

## Setup for Cloudflare R2
- Go to: https://dash.cloudflare.com/sign-up/r2 and create an account
- Go to the R2 buckets page: https://dash.cloudflare.com/?to=/:account/r2 and click "Create bucket"
- Name the bucket whatever you want, just copy this name down.  You can leave location on automatic. Then click "Create Bucket".
- Go to the R2 API Token page https://dash.cloudflare.com/?to=/:account/r2/api-tokens and click "Create API token"
- Name the token anything you like, for permissions you can just do "
Object Read & Write",  under "Specify bucket(s)" click "Apply to specific buckets only
" and select the bucket you just created.  Leave the other items at default and click "Create API Token"
- Copy the access key id, secret access key, and the "jurisdiction-specific endpoints for S3 Client" url (this is the service url).
- Enter these 3 items in KeePassSync options.


## Setup for Backblaze B2

- Go to: https://www.backblaze.com/cloud-storage and signup (make sure to choose a region from the dropdown below and on the right side of the "Sign Up Now" button (or you will get an odd error).
- Once signed in go to: https://secure.backblaze.com/b2_buckets.htm and click "Create a Bucket".  Name it whatever you would like,  leave "Files in Bucket are" set to private, set the encryption to enabled if you like (not required) do NOT enable object lock.  click "Create a Bucket"
- One the bucket is created you will be taken back to your bucket list page.  From here copy the endpoint url (ie: s3.us-west-007.backblazeb2.com).
- Next click the "How to connect to this bucket" in the box showing that bucket on the bucket list.  click "Create an app key".  
- On the app key page click "Add a New Application Key" give the key whatever name you want.  You just need to copy the "Key ID" and the "Application key" and put these into keepass sync (along with the endpoint URL from above).
- Enter these 3 items in KeePassSync options.


## Standard Amazon s3 setup
You can give the user the ability to create buckets then you can just create the user and KPSync will do the rest, but a more secure way is to create the bucket yourself.  There may be an easier way to do this but this will work:

### Generate an access key user and get users ARN:
- Login to AWS on your account click your name in the upper right, go to Security Credentials.
- Click users under Access Management on the left.
- Click "Create User" button on the right.
- Create a random username, it doesn't matter.
- Click "Attach Policies Directly" and then just click "next" at the bottom right to not assign any properties.
- Click "Create User"
- Click on the user on the users list
- Click "Security Credentials" on that page.
- CLick "Create Access Key" near the bottom right"
- Click "Other"
- For "Description tag value" but whatever you like, ie "for keepass".
- On the next page Copy the access key ID and secret to paste into keepass later, copy their ARN from the upper left on the page (should start with "arn:aws....")


### Add the users permissions to a new (or existing) S3 storage bucket	

- Next on the top left click "Services" go to storage and S3.
- Click "Create Bucket", git it a name under bucket name, leave ACL's disabled, block all public access (checked by default)
- Leave all other settings default and click create bucket.
- Click on the new bucket in the list, click permissions, click edit next to bucket policy
- Edit the bucket policy below replacing the one spot it says `USER_ARN_HERE` with your users arn, and then under resource replace the two `BUCKET_NAME_HERE` strings with the new buckets name, BE SURE not to edit the items before or after `BUCKET_NAME_HERE` they are required.
```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "PrimaryAccess",
            "Effect": "Allow",
            "Principal": {
                "AWS": "USER_ARN_HERE"
            },
            "Action": [
                "s3:ListBucket",
                "s3:PutObjectAcl",
                "s3:GetObject",
                "s3:GetObjectAcl",
                "s3:PutObject"
            ],
            "Resource": [
                "arn:aws:s3:::BUCKET_NAME_HERE/*",
                "arn:aws:s3:::BUCKET_NAME_HERE"
            ]
        }
    ]
}
```

- Click Save and now try putting the bucket name and key information into keepass sync!