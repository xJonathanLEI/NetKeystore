# NetKeystore

The library is available as a NuGet package [here]().

## Introduction

This is a light-weight library that's only responsible for dealing with [Ethereum keystore files](https://github.com/ethereum/wiki/wiki/Web3-Secret-Storage-Definition) written in C#.

## Using the Package

### Adding to your project

Add the package by using the following command:

    $ dotnet add package NetKeystore

### Using it in your code

## Limitations

Currently the functionality of the library is quite limited with the following known limitations:

- Can only decrypt keystore files; no way to creat one
- Only supports `scrypt` as the key derivation function

I will try to perfect the library in the future, or you can help me do it together ;)

## Contribution

Contributions are welcomed! You may post issues on GitHub, or better, post pull requests for this project.

You can also contribute test data as part of the unit test!