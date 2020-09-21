# Redis配置示例

> Framework：`app.config`、`web.config`

```
<appSettings>
	<add key="RedisServer" value="127.0.0.1:6379" />
	<add key="RedisPwd" value="" />
</appSettings>
```

> Net Core：`appsettings.json`

```
{
  "Redis": {
    "endPoints": "127.0.0.1:6379",
    "pwd": ""
  }
}
```