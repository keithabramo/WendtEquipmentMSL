﻿using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using WendtEquipmentTracking.Common.DTO;

namespace WendtEquipmentTracking.Common
{
    public static class ActiveDirectoryHelper
    {
        private const string DOMAIN_NAME = "DBSERVER";

        public static string CurrentUserUsername()
        {
            var fullUsername = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            var usernameParts = fullUsername.Split('\\');

            var username = string.Empty;
            if (usernameParts.Length == 2)
            {
                username = usernameParts[1];
            }

            return username;
        }

        public static string DisplayName(string username)
        {
            string displayName = username;

            if (!string.IsNullOrWhiteSpace(username))
            {

                try
                {
                    var principalContext = new PrincipalContext(ContextType.Domain, DOMAIN_NAME);

                    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(
                        principalContext,
                        IdentityType.SamAccountName,
                        DOMAIN_NAME + "\\" + username);


                    var user = mapToUser(userPrincipal);
                    displayName = user.DisplayName;

                }
                catch
                {
                }
            }

            return displayName;
        }

        public static ActiveDirectoryUser GetUser(string username)
        {
            var user = new ActiveDirectoryUser();

            if (!string.IsNullOrWhiteSpace(username))
            {
                try
                {
                    var principalContext = new PrincipalContext(ContextType.Domain, DOMAIN_NAME);

                    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(
                        principalContext,
                        IdentityType.SamAccountName,
                        DOMAIN_NAME + "\\" + username);

                    user = mapToUser(userPrincipal);

                }
                catch
                {
                }
            }
            user.Role = UserRoles.ReadWrite;
            return user;
        }



        public static IEnumerable<ActiveDirectoryUser> ActiveDirectoryUsers(string searchString)
        {
            IList<ActiveDirectoryUser> users = new List<ActiveDirectoryUser>();
            string userName = ActiveDirectoryHelper.CurrentUserUsername();

            try
            {
                var principalContext = new PrincipalContext(ContextType.Domain, DOMAIN_NAME, "DC=corp,DC=ene,DC=com");

                //Get the current users's principal
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(
                    principalContext,
                    IdentityType.SamAccountName,
                    DOMAIN_NAME + "\\" + userName);

                // if user is found
                if (userPrincipal != null)
                {

                    //Query by example display name and username (aka Name)
                    UserPrincipal qbeDisplayNamePrincipal = new UserPrincipal(principalContext);
                    qbeDisplayNamePrincipal.DisplayName = "*" + searchString + "*";
                    UserPrincipal qbeUsernamePrincipal = new UserPrincipal(principalContext);
                    qbeUsernamePrincipal.SamAccountName = "*" + searchString + "*";

                    //Prepare
                    PrincipalSearcher displayNameSearcher = new PrincipalSearcher(qbeDisplayNamePrincipal);
                    PrincipalSearcher usernameSearcher = new PrincipalSearcher(qbeUsernamePrincipal);

                    PrincipalSearchResult<Principal> foundFullNameSearcher = null;
                    if (searchString.Contains(" "))
                    {
                        var searchStrings = searchString.Split(' ');

                        if (!string.IsNullOrWhiteSpace(searchStrings[0]) || !string.IsNullOrWhiteSpace(searchStrings[1]))
                        {
                            UserPrincipal qbeFullNamePrincipal = new UserPrincipal(principalContext);

                            if (!string.IsNullOrWhiteSpace(searchStrings[0]))
                            {
                                qbeFullNamePrincipal.GivenName = "*" + searchStrings[0] + "*";
                            }
                            if (!string.IsNullOrWhiteSpace(searchStrings[1]))
                            {
                                qbeFullNamePrincipal.Surname = "*" + searchStrings[1] + "*";
                            }

                            PrincipalSearcher fullNameSearcher = new PrincipalSearcher(qbeFullNamePrincipal);

                            foundFullNameSearcher = fullNameSearcher.FindAll();
                        }
                    }

                    //Execute
                    var foundDisplayNameUsers = displayNameSearcher.FindAll();
                    var foundUsernameUsers = usernameSearcher.FindAll();

                    //Union
                    var foundUsers = foundUsernameUsers.Union(foundDisplayNameUsers);
                    if (foundFullNameSearcher != null)
                    {
                        foundUsers = foundUsers.Union(foundFullNameSearcher);
                    }

                    //Iterate over users found
                    foreach (UserPrincipal foundUser in foundUsers.ToList())
                    {
                        users.Add(mapToUser(foundUser));
                    }
                }
            }
            catch
            {
            }

            return users
                .GroupBy(u => u.Username)
                .Select(g => g.First())
                .ToList();
        }

        private static ActiveDirectoryUser mapToUser(UserPrincipal userPrincipal)
        {
            var user = new ActiveDirectoryUser
            {
                Username = userPrincipal.SamAccountName,
                FirstName = userPrincipal.GivenName,
                MiddleName = userPrincipal.MiddleName,
                LastName = userPrincipal.Surname,
                Email = userPrincipal.EmailAddress,
                Role = userPrincipal.GetAuthorizationGroups().Any(ag => ag.Name == "ReadWrite") ? UserRoles.ReadWrite : UserRoles.ReadOnly
            };

            return user;
        }
    }
}