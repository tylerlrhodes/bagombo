# Bagombo

Simple blogging software using ASP.NET Core MVC and Entity Framework Core

See it in action at [www.bagombo.org](http://www.bagombo.org)

Contributors welcome : )

### General Notes on Bagombo:
* It's still being developed, "Alpha" releases may not provide an easy database upgrade path from one to another
* Features so far include:
    * Multi-Author Support
    * Categorize posts by "Feature" (being renamed to Topic) or Category
    * Twitter, Facebook and Local Authentication
    * Full text search of posts
    * Author posts in markdown
    * MIT License 
* Features planned include:
    * MetaWebLog API Support
    * SQL Server or SQLite backend support (currently just SQL Server)
    * Support for themes and customization

### 0.2.2 Alpha - Released
* [Releases](http://github.com/tylerlrhodes/bagombo/releases)
* Breaking changes from 0.2.1 in Data Layer, would require a manual update to 0.2.2, some more changes coming in 0.2.3 (Data layer stability and better upgrades being worked on)
* Mostly some bug fixes and minor tweaks
* No functionality added
* Don't update to this from 0.2.1 as its probably not worth the manual post export / copy paste procedure required for the update.

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
* Implement Repository pattern for data access (or CQRS)
* More unit tests
* GUI enhancements and more color!
* Easier to install

### Goals for 0.5
* First Beta Release and Data Layer Stability

### Goals for 1.0:
* TBD

MIT license





