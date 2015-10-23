#  Web Store

This repository contains the source code for an online store web-site. 

The web-site has a web-interface for administration of its content, provides possibility for its user to checkout via PayPal.

## Acknowledgements

The list of used libraries is shown below:

* [ASP.NET MVC Framework][ASP.NET MVC Framework link]
* [MongoDB .NET Driver][MongoDB .NET Driver link]
* [Ninject][Ninject link]
* [jQuery][jQuery link]
* [Bootstrap][Bootstrap link]
* [fancyBox][fancyBox link]
* [log4net][log4net link]
* [Bcrypt.NET][Bcrypt.NET link]
* [PayPal .NET SDK][PayPal .NET SDK link]

As a database management system MongoDB was used.

## Building

In order to build the project you'll need Visual Studio 2015 or newer and IIS 10 Express server or newer.

To download all the dependencies use the following Nuget command for each of two projects listed in the repository:

```
$ nuget install packages.config
```

## License

The source code is published under [GPLv2][GPLv2 link] license.

  [GPLv2 link]: https://www.gnu.org/licenses/gpl-2.0.html

  [ASP.NET MVC Framework link]: http://www.asp.net/mvc/
  [MongoDB .NET Driver link]: https://mongodb.github.io/mongo-csharp-driver/
  [Ninject link]: http://www.ninject.org/
  [jQuery link]: http://jquery.com/
  [Bootstrap link]: http://getbootstrap.com/
  [fancyBox link]: http://fancyapps.com/fancybox/
  [log4net link]: https://logging.apache.org/log4net/
  [Bcrypt.NET link]: https://bcrypt.codeplex.com/
  [PayPal .NET SDK link]: https://paypal.github.io/PayPal-NET-SDK/
