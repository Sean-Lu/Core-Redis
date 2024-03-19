## 简介

> 基于[`StackExchange.Redis`](https://github.com/StackExchange/StackExchange.Redis)封装的Redis客户端

- 支持`.Net Framework`、`.Net Core`
- 支持同步、异步操作（async\await）
- 使用简单，方便扩展
- **常见redis数据类型**：
  - `String`：字符串
  - `List`：列表
  - `Set`：集合
  - `SortedSet`：有序集合
  - `Hash`：哈希

## Packages

| Package                                                            | NuGet Stable                                                                                                              | NuGet Pre-release                                                                                                            | Downloads                                                                                                                  | MyGet                                                                                                                                                 |
| ------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Sean.Core.Redis](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis](https://img.shields.io/nuget/v/Sean.Core.Redis.svg)](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis](https://img.shields.io/nuget/vpre/Sean.Core.Redis.svg)](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis](https://img.shields.io/nuget/dt/Sean.Core.Redis.svg)](https://www.nuget.org/packages/Sean.Core.Redis/) | [![Sean.Core.Redis MyGet](https://img.shields.io/myget/sean/vpre/Sean.Core.Redis.svg)](https://www.myget.org/feed/sean/package/nuget/Sean.Core.Redis) |

## Nuget包引用

> **Id：Sean.Core.Redis**

- Package Manager

```powershell
PM> Install-Package Sean.Core.Redis
```

## Redis配置示例

> **如果Redis是集群部署的话，地址用","分隔即可。**

- .NET Framework：`app.config`、`web.config`

```xml
<appSettings>
    <add key="RedisEndPoints" value="127.0.0.1:6379" />
    <add key="RedisPassword" value="" />
    <add key="RedisDefaultSerializeType" value="0" />
</appSettings>
```

- .NET Core：`appsettings.json`

```json
{
  "Redis": {
    "EndPoints": "127.0.0.1:6379",
    "Password": "",
    "DefaultSerializeType": 0,
  }
}
```

## 使用示例

> 项目：`examples\Example.NetCore`

1. Redis初始化
- 不使用依赖注入

```csharp
// 示例1：
RedisManager.Initialize(configuration);

// 示例2：
RedisManager.Initialize(new RedisClientOptions
{
  EndPoints = "127.0.0.1:6379",
  Password = string.Empty,
  DefaultSerializeType = SerializeType.Json
});
```

- 使用依赖注入

```csharp
services.AddRedis(configuration);
```

2. Redis使用示例
- string => 最简单的用法

```csharp
var cacheKey = "test";
Console.WriteLine($"添加string缓存：{RedisHelper.StringSet(cacheKey, new TestModel { Id = 1001, Name = "Sean" }, TimeSpan.FromSeconds(20))}");
Console.WriteLine($"同步获取缓存：{JsonHelper.Serialize(RedisHelper.StringGet<Test>(cacheKey))}");
Console.WriteLine($"异步获取缓存：{JsonHelper.Serialize(RedisHelper.StringGetAsync<Test>(cacheKey).Result)}");
Console.WriteLine($"手动删除缓存：{RedisHelper.KeyDelete(cacheKey)}");
```

- list => 实现队列（先进先出）

```csharp
var cacheKeyListQueue = "testListQueue";
var list = new List<Test>
{
    new TestModel { Id = 1002, Name = "aaa" },
    new TestModel { Id = 1003, Name = "bbb" },
    new TestModel { Id = 1004, Name = "ccc" },
};
Console.WriteLine($"添加list缓存：{RedisHelper.ListRightPush(cacheKeyListQueue, list)}");
Console.WriteLine($"设置缓存超时时间：{RedisHelper.KeyExpire(cacheKeyListQueue, TimeSpan.FromSeconds(20))}");
Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<Test>(cacheKeyListQueue))}");
Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<Test>(cacheKeyListQueue))}");
Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<Test>(cacheKeyListQueue))}");
```

- list => 实现堆栈（先进后出）

```csharp
var cacheKeyListStack = "testListStack";
Console.WriteLine($"添加list缓存：{RedisHelper.ListLeftPush(cacheKeyListStack, list)}");
Console.WriteLine($"设置缓存超时时间：{RedisHelper.KeyExpire(cacheKeyListStack, TimeSpan.FromSeconds(20))}");
Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<Test>(cacheKeyListStack))}");
Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<Test>(cacheKeyListStack))}");
Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<Test>(cacheKeyListStack))}");
```
