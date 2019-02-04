<h1 align="center">
  Administrative Organizer
  <br>
  API
  <br>
</h1>

<p align="center">
  <a href="#about">About</a> •
  <a href="#getting-started">Getting Started</a> •
  <a href="#restful-urls">RESTful URLS</a> •
  <a href="#api-responses">API Responses</a> •
  <a href="#error-handling">Error Handling</a> •
  <a href="#versions">Versions</a> •
  <a href="#api-endpoints">API Endpoints</a> •
  <a href="#authors">Authors</a>
</p>

## About
This document provides simple information regarding our API, and its functionality.

AO API was first started in April 2018 by BalenD, Mikkel and Jason, and is a collaborative project made with learning as its main purpose.

___This project is mainly made for us to gain knowlegde in the various technologies, while also being effective in best practices.___

## Getting Started

## RESTful URLS

## API Responses

## Error Handling

## Versions
* As of now the API should be v2, the version changes when major changes happens.
* This API is derived from an older [API](https://github.com/BalenD/AO-Api-Prototype-Project) of ours.

## API endpoints
#### User Resources
- <code>GET</code> users
- <code>POST</code> users
- <code>GET</code> users:id
- <code>PATCH</code> users/:id
- <code>DELETE</code> users/:id
#### Employee Resources
- <code>GET</code> employee
- <code>POST</code> employee
- <code>GET</code> employee/:id
- <code>PATCH</code> employee/:id
- <code>DELETE</code> employee/:id
#### Job Resources
- <code>GET</code> employee/:id/job
- <code>PATCH</code> employee/:id/job
#### Schedule Resources
- <code>GET</code> employee/:id/schedules
- <code>POST</code> employee/:id/schedules
- <code>GET</code> employee/:id/schedules/schedules:id
- <code>PATCH</code> employee/:id/schedules/schedules:id
- <code>DELETE</code> employee/:id/schedules/schedules:id
#### Wallet Resources
- <code>GET</code> employee/:id/wallet
- <code>PATCH</code> employee/:id/wallet
#### Workhours Resources
- <code>GET</code> employee/:id/workhours
- <code>PATCH</code> employee/:id/workhours

## Authors
- BalenD - [BalenD](https://github.com/BalenD)
- Mikkel - [x-mfh](https://github.com/x-mfh)
- Jason - [Sabeniano](https://github.com/Sabeniano)
