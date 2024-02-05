# Amazon S3 Setup

You can give the user the ability to create buckets then you can just create the user and KPSync will do the rest, but a more secure way is to create the bucket yourself.  There may be an easier way to do this but this will work:

## Generate an access key user and get users ARN:
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


## Add the users permissions to a new (or existing) S3 storage bucket	

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