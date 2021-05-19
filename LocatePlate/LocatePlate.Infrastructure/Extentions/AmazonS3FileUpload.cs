using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.Infrastructure.Extentions
{
    public class AmazonS3FileUpload
    {
        public async void UploadFile(string keyName, string filePath)
        {
            string bucketName = "locateplatesignature/testing";
            string accesskey = "AKIA6JJG4IG46VXAOUFQ";
            string secretkey = "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";

            var client = new AmazonS3Client(accesskey, secretkey, Amazon.RegionEndpoint.USEast1);

            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    FilePath = filePath,
                    ContentType = "application/pdf"
                };

                PutObjectResponse response = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }
    }
}
