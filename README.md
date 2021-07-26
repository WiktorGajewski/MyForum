# MyForum

### Target
Create simple ASP.NET Core web app: **Internet forum**. 
Using Razor Pages.

###Already implemented
- Users
- Guilds (a.k.a. Groups)
- Authentication (local accounts)
- Authorization (Ranks)
- Ranks for Users: Leader(a.k.a. Admin), Guildmaster(a.k.a. manager of a group), Novice(a.k.a. normal user)
- option to Manage Ranks for users with Rank Leader
- Added Captcha challenge from Goggle during registration 
  (and an option to turn it off in appsettings.json)
- Added MainForum - first forum page and possibility to send Messages

###To do list
- [x] assign Guildmasters to Guilds
- [x] every Guild should have it's own forum
- [x] Guildmasters should be able to invite users to Guilds
- [ ] Likes
- [ ] Prestige Points system: likes add prestige points
