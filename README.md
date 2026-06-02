# SweetyCommon
这是一个公共类库，封装了Web应用日常开发中一些常用的功能。目前这个库还处在搭建初期，搭建完成后会合并（删除）一些 git commit，所以暂时让这个库处于私有状态。

# 变更日志

## 1.0.5
- 从 1.0.5 版本开始`IHash`的内部实现`InternalHash`使用的默认字符集从`Unicode`变更为`utf-8`。

## 1.0.6
- 增加 FixedCapacityParameterList debug 代码，辅助排查 SqlParameter 没有从 SqlParameterCollection 释放的问题。