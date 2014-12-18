using System;
using Amazon;
using Amazon.S3.Model;

namespace bucketcleaner
{
	public class Program {
		public static void Main(string[] args)
		{
			string accessKey = Environment.GetEnvironmentVariable ("AWS_ACCESS_KEY"); 
			string secretKey = Environment.GetEnvironmentVariable ("AWS_SECRET_KEY");
			int hoursToKeep = 12;

			if (args.Length == 0) {
				throw new ApplicationException ("The bucket name must be the first argument");
			}

			if (args.Length > 1) {
				hoursToKeep = int.Parse(args [1]);
			}

			CleanBucket(accessKey, secretKey, args[0], TimeSpan.FromHours(hoursToKeep));
		}

		public static void CleanBucket(string accessKey, string secretKey, string bucket, TimeSpan t) {
			using(var client = new Amazon.S3.AmazonS3Client(accessKey,secretKey,RegionEndpoint.USEast1)) {			
				string marker = null;				
				while(true) {
					var loReq = new ListObjectsRequest() { Marker = marker, BucketName = bucket};
					var loRes = client.ListObjects(loReq);

					foreach(var o in loRes.S3Objects)
					{				
						if(o.LastModified <= DateTime.Now.Subtract(t))
						{				
							client.DeleteObject(bucket, o.Key);
							Console.WriteLine ("Deleted " + o.Key);
						}				
					}

					if(loRes.IsTruncated) {
						marker = loRes.NextMarker;
					} 
					else {
						break;
					}			
				}
			}	
		}
	}
}