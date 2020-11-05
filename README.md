## 简介

> 基于[`StackExchange.Redis`](https://github.com/StackExchange/StackExchange.Redis)封装的Redis客户端

- 支持`.Net Framework`、`.Net Core`
- 支持同步、异步操作（async\await）
- 使用简单，方便扩展

## Packages

| Package | NuGet Stable | NuGet Pre-release | Downloads | MyGet |
| ------- | ------------ | ----------------- | --------- | ----- |
| [Sean.Core.Redis](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis](https://img.shields.io/nuget/v/Sean.Core.Redis.svg)](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis](https://img.shields.io/nuget/vpre/Sean.Core.Redis.svg)](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis](https://img.shields.io/nuget/dt/Sean.Core.Redis.svg)](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis MyGet](https://img.shields.io/myget/sean/vpre/Sean.Core.Redis.svg)](https://www.myget.org/feed/sean/package/nuget/Sean.Core.Redis) |

## Nuget包引用

> **Id：Sean.Core.Redis**

- Package Manager

```
PM> Install-Package Sean.Core.Redis
```

## Redis配置示例

> **如果Redis是集群部署的话，地址用","分隔即可。**

- Framework：`app.config`、`web.config`

```
<appSettings>
	<add key="RedisServer" value="127.0.0.1:6379" />
	<add key="RedisPwd" value="" />
</appSettings>
```

- Net Core：`appsettings.json`

```
{
  "Redis": {
    "endPoints": "127.0.0.1:6379",
    "pwd": ""
  }
}
```

## 使用示例

> 项目：test\Demo.NetCore