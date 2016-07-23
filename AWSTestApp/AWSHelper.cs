using System.Collections.Generic;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;

namespace AWSTestApp
{
    public class AWSHelper
    {
        public static IEnumerable<User> ListIamUsers()
        {
            var users = new List<User>();
            var credentials = new StoredProfileAWSCredentials("dna_user_1");

            using (var iamServiceClient = new AmazonIdentityManagementServiceClient(credentials))
            {
                var usersRequest = new ListUsersRequest();
                var usersResponse = iamServiceClient.ListUsers(usersRequest);
                users.AddRange(usersResponse.Users);

                while (usersResponse.IsTruncated)
                {
                    usersRequest.Marker = usersResponse.Marker;
                    usersResponse = iamServiceClient.ListUsers(usersRequest);
                    users.AddRange(usersResponse.Users);
                }
            }
            return users;
        }

        public static IEnumerable<AccessKeyMetadata> ListUserAccessKeys(User user)
        {
            var keys = new List<AccessKeyMetadata>();
            var credentials = new StoredProfileAWSCredentials("dna_user_1");

            using (var iamServiceClient = new AmazonIdentityManagementServiceClient(credentials))
            {
                var request = new ListAccessKeysRequest { UserName = user.UserName };
                var response = iamServiceClient.ListAccessKeys(request);
                keys.AddRange(response.AccessKeyMetadata);

                while (response.IsTruncated)
                {
                    request.Marker = response.Marker;
                    response = iamServiceClient.ListAccessKeys(request);
                    keys.AddRange(response.AccessKeyMetadata);
                }
            }
            return keys;
        }

        public static IEnumerable<Group> ListGroups()
        {
            var keys = new List<Group>();
            var credentials = new StoredProfileAWSCredentials("dna_user_1");

            using (var iamServiceClient = new AmazonIdentityManagementServiceClient(credentials))
            {
                var request = new ListGroupsRequest();
                var response = iamServiceClient.ListGroups(request);
                keys.AddRange(response.Groups);

                while (response.IsTruncated)
                {
                    request.Marker = response.Marker;
                    response = iamServiceClient.ListGroups(request);
                    keys.AddRange(response.Groups);
                }
            }
            return keys;
        }

        public static IEnumerable<Instance> ListEc2Instances()
        {
            var instances = new List<Instance>();
            var credentials = new StoredProfileAWSCredentials("dna_user_1");
            using (var client = new AmazonEC2Client(credentials, RegionEndpoint.USWest2))
            {
                var request = new DescribeInstancesRequest();
                var response = client.DescribeInstances(request);
                response.Reservations.ForEach(r => instances.AddRange(r.Instances));
            }
            return instances;
        }

        public static IEnumerable<KeyPairInfo> ListEc2KeyPairsByInstance(Instance instance)
        {
            var credentials = new StoredProfileAWSCredentials("dna_user_1");
            using (var client = new AmazonEC2Client(credentials, RegionEndpoint.USWest2))
            {
                var request = new DescribeKeyPairsRequest() { KeyNames = new List<string>(new[] { instance.KeyName }) };
                var response = client.DescribeKeyPairs(request);
                return response.KeyPairs;
            }
        }
    }
}