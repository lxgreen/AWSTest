using System;
using System.Linq;
using Amazon.EC2.Model;
using Amazon.IdentityManagement.Model;

namespace AWSTestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("IAM Users:");
            ListIamUsers();
            ListGroups();
            ListEc2Instances();

            Console.ReadLine();
        }

        private static void ListGroups()
        {
            Console.WriteLine("IAM Group data:");
            var groups = AWSHelper.ListGroups();
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Arn} | {group.CreateDate} | {group.GroupId} | {group.GroupName} | {group.Path}");
            }
        }

        private static void ListEc2KeyPairsByInstance(Instance instance)
        {
            Console.WriteLine($"EC2 instance {instance.InstanceId} keys data:");

            var keyPairs = AWSHelper.ListEc2KeyPairsByInstance(instance);
            foreach (var keyPair in keyPairs)
            {
                Console.WriteLine($"{keyPair.KeyName} | {keyPair.KeyFingerprint}");
            }
        }

        private static void ListEc2Instances()
        {
            Console.WriteLine("EC2 instance data:");
            var instances = AWSHelper.ListEc2Instances();
            foreach (var i in instances)
            {
                Console.WriteLine($"{i.Architecture} | {i.ImageId} | {i.InstanceId} " +
                    $"| {i.InstanceType} | {i.KernelId} | {i.KeyName} " +
                    $"| {i.LaunchTime} | {i.Platform} | {i.PrivateIpAddress} | {i.PublicIpAddress}");
                ListEc2KeyPairsByInstance(i);
            }
        }

        private static void ListIamUserAccessKeys(User user)
        {
            var keys = AWSHelper.ListUserAccessKeys(user);
            foreach (var key in keys)
            {
                Console.WriteLine($"{key.UserName} | {key.AccessKeyId} | {key.Status} | {key.CreateDate}");
            }
        }

        private static void ListIamUsers()
        {
            var users = AWSHelper.ListIamUsers().ToList();
            foreach (var user in users)
            {
                Console.WriteLine(
                    $@"{user.UserName} | {user.Arn} | {user.CreateDate} | {user.PasswordLastUsed} |" +
                    $@" {user.Path} | {user.UserId}");
                Console.WriteLine($"{user.UserName} access key data:");
                ListIamUserAccessKeys(user);
            }
        }
    }
}