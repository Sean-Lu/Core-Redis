# 更新日志

## [2.0.4] - 2020-09-17

### 修改

- 解决连接数过多会导致超时的严重bug：`ConcurrentDictionary<int, IDatabase>`

## [2.0.3-beta2020053101] - 2020-05-31

### 修改

- 优化Redis初始化逻辑

## [2.0.2] - 2020-05-31

## [2.0.0] - 2019-05-31

### 新增

* 支持选择指定索引的 `Redis` 库：`RedisHelper.GetDatabase(int db)`

## [1.0.1] - 2018-12-04

## [1.0.0] - 2018-11-27

### 新增

* 发布第1个版本，基于 `StackExchange.Redis` 对Redis进行操作

```
注： `ServiceStack.Redis` 从 `v4.0` 开始已经成为商业产品，不再完全免费，好在是开源的，主要限制免费配额功能在 `ServiceStack.Text` 类库下的  `LicenseUtils.cs` 文件中，仅需从 `GitHub` 上下载源码后添加一行代码重新编译即可解除限制。
```