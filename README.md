# external-configuration-store
A small component to retrieve configuration settings from an external store

## Context and Problem
After using the `ConfigurationManager` and `CloudConfigurationManager` to get application settings, I've needed a solution that allows me not to redeploy or restart applications to obtain new settings or updated settings.

Following MSDN Documentation about Cloud Design Pattern, I've read this article [External Configuration Store Pattern](https://msdn.microsoft.com/en-us/library/dn589803.aspx) and started an implementation of an external configuration store from their sample : [Cloud Design Patterns – Sample Code](https://www.microsoft.com/en-us/download/details.aspx?id=41673).

## Features

