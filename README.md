# bagombo

Simple blogging software using ASP.NET Core MVC and Entity Framework Core

See it in action at [www.bagombo.org](http://www.bagombo.org)

### 0.2 Alpha is released
* It's only tested on Windows Server 2016 and SQL Server 2016
* This is basically useable as a blog now
* The home page is really the only spot where modifying code is needed, by updating /Views/Home/Index.html -- basically to update links to entries instead of taking what was left over
* You can run it by compiling from Visual Studio 2017 and publishing it to a server, then set it up in IIS
* There is a CreateDb.sql file under src, you have to create the database but can run this to setup the tables
* For the search to work you need to create a full-text index on the BlogPost table
* You need to setup your connection string either in appsettings.json, user secrets or an environment variable depending on your environment.  Not sure the best way, but I use an environment variable on the server
* Change appsettings.json to seed the Db with an admin user besdies my default
* Use the user manager to make an author
* Hopefully the rest you can figure out
* The author profile page doesn't work yet, if you want you can setup a post and link to it that way

### Goals for 0.3
* Bug Fixes
* Better error handling
* A few unit tests
* Easier to install
* Script the database setup
* Binary release format
* Minor GUI fixups
* SEO
* Add Author Profile Page

### Goals for 0.4
* Implement Repository patter for data access (or CQRS)
* More unit tests
* GUI enhancements and more color!
* Easier to install

### Goals for 1.0:
* TBD

MIT license

## Roadmap:
### * 0.3 - code cleanup and refactoring, bug fixes - June 1st
### * 0.4 - Core Feature enhancements - July 1st
### * 0.5 - Data Layer Stabilization - August 1st
### * 0.6 - Core HTML and SEO 
### * 0.7 - Front End Enhancement and Themes - Nov 1st
### * 0.8 - code cleanup and refactoring - Jan 1st
### * 0.9 - Blog data portability - Feb 1st
### * 1.0 - Bug fixes




