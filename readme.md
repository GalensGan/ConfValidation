# ConfValidation

ConfValidation 是一个灵活的、配置式的验证器，相较于 FluentValidation 需要单独定义验证模型不同，ConfValidation 在使用时，直接通过初始化器来定义，清晰的树形结构验证，简单直观，所见即所得。

## 安装

目前未发布 Nuget 包，有需要的，直接下载编译使用。

## 使用

在使用 `ConfValidation` 时，可以根据需求任意组全

### 常用示例

``` csharp
// 数据
var myClass = new Class()
{
    Name = "class 1",
    Order = 1,
    // 嵌套对象
    Address = new Address()
    {
        Email = "abc@china.com"
    },
};

// 验证
var vdResult = myClass.Validate(new VdObj // 顶层是对象类型，使用 VdObj 验证器包裹其它验证器
{
    // 通过验证器名称 IsString 来验证数据 myClass.Name
    { "$Name","IsString"},
    // 逻辑且
    { "$Order",new And(){
        VdNames.IsNumber,
        new LessThan(2),
        new InRange(0,10),
        new Function<int>(x=>x>1)
    } }, 
    { ()=>myClass.Address,new VdObj{
        { "$Email",new IsString()}
    } } 
});

Assert.IsTrue(vdResult);
Assert.IsTrue(vdResult.Ok);

// 多次验证
var vdResult2 = myClass.Name.Validate(VdNames.IsNumber);
Assert.IsFalse(vdResult2.Ok);
var vdResult3 = myClass.Students[0].Scores["Math"].Score.Validate(new And()
{
    new GreaterThan(60),
    VdNames.IsNumber
});
Assert.IsTrue(vdResult3);
```

### 完整用法示例

``` c#
// 数据
var myClass = new Class()
{
    Name = "class 1",
    Order = 1,
    // 嵌套对象
    Address = new Address()
    {
        Email = "abc@china.com"
    },
    // 列表
    Students = new List<Student>()
    {
       new Student(){ Name = "Jack",
           // 数组
           Subjects = new Subject[] {
                new Subject(){Name = "English",Score = 80},
                new Subject(){Name = "Math",Score=90}
           },
           // 字典
           Skills = new Dictionary<string, double>()
           {
               { "cook",80 },
               { "football",60}
           },
           // 字典
           Scores = new Dictionary<string, Subject>()
           {
               {"English", new Subject(){Score = 80} },
               {"Math", new Subject(){Score=90} }
           }
       },
       new Student(){ Name = "Timi",
           Subjects = new Subject[] {
                new Subject(){Name = "English",Score = 80},
                new Subject(){Name = "Math",Score=90}
           },
           Skills = new Dictionary<string, double>()
           {
               { "cook",80 },
               { "football",60}
           },
            Scores = new Dictionary<string, Subject>()
           {
               {"English", new Subject(){Score = 80} },
               {"Math", new Subject(){Score=90} }
           }
       }
    }
};

// 整体一次性验证
var vdResult = myClass.Validate(new VdObj // 顶层是对象类型，使用 VdObj 验证器包裹其它验证器
{
    // 通过验证器名称 IsString 来验证数据 myClass.Name
    { "$Name","IsString"},
    // 原理同上，VdNames 中定义了一些系统验证器常量
    { "$Name",VdNames.IsString},
    // 通过传入验证器来验证
    { "$Name",new IsString()},
    // 自定义错误消息
    { "$Name",new IsString(),"myClass.Name 不是字符串"},
    // 自定义验证器初始化参数
    { "$Name",new IsString(){
        MaxLength = 10,
        Required = true,
        },"myClass.Name 的最大长度为 10"
    },
    // 传入 Func<T> 进行自主验证
    { "$Name",x=>x=="class 1"},
    // 通过表达方式传入验证值的路径，不用手动输入字符串，减少输入失误
    { ()=>myClass.Name,VdNames.IsString},
    { ()=>myClass.Name,x=>x=="class 1"},
    // 同时输入多个验证器
    { ()=>myClass.Order,new Validator[]{
        VdNames.IsNumber,
        new LessThan(2),
        new InRange(0,10),
        new Function<int>(x=>x>1)          
    } },
    // 逻辑且
    { "$Order",new And(){
        VdNames.IsNumber,
        new LessThan(2),
        new InRange(0,10)
    } },
    // 逻辑或
    { "$Order",new Or(){
        VdNames.IsNumber,
        new LessThan(2),
        new InRange(0,10)
    } },
    // 传入具体的值进行验证，传入具体值时，该验证器组就无法重用，因为特定值已经记录到了验证器中
    { myClass.Name,VdNames.IsString},

    // 下面是嵌套验证
    // 嵌套验证可以对上述基本使用方式进行任意组合

    // 验证嵌套对象
    { ()=>myClass.Address.Email,new IsString()},
    { ()=>myClass.Address,new VdObj{
        { "$Email",new IsString()}
    } },
    { "$Address.Email",new IsString() },

    // 验证嵌套数组
    { ()=>myClass.Students[0].Scores["Math"].Score,new GreaterThan(60)},
    // 验证第一个学生所有科目的分数
    // 集合中的值分为 Key 和 Value,所以使用 Scores[].Value
    { "$Students[0].Scores[].Value.Score",new EachElement(){
        VdNames.IsNumber,
        new GreaterThan(60),
    } },
    // 不断向下嵌套验证
    { "$Students[0]",new VdObj{
        { "$Name",new Equals("Jack")},
        { "$Skills[cook]",new Or(){
            VdNames.IsNumber,
            new GreaterThan(60)
        } }
    } },
});

Assert.IsTrue(vdResult);
Assert.IsTrue(vdResult.Ok);

// 多次验证
var vdResult2 = myClass.Name.Validate(VdNames.IsNumber);
Assert.IsFalse(vdResult2.Ok);
var vdResult3 = myClass.Students[0].Scores["Math"].Score.Validate(new And()
{
    new GreaterThan(60),
    VdNames.IsNumber
});
Assert.IsTrue(vdResult3);
```

### 语法

#### 扩展方法

每一个数据实体通过扩展方法来调用验证器，扩展方法有两个重载，分别是：

``` c#
// 单个验证器
public static ValidateResult Validate<T>(this T data, Validator validator, ValidateOption options = ValidateOption.None)
{   
}

// 验证器数组
public static ValidateResult Validate<T>(this T data, Validator[] validators, ValidateOption options = ValidateOption.None)
{    
}
```

因此，可以通过如下方式调用 `ConfValidator` 进行验证

``` c#
model.Validate(validator1,option);

// 或者
model.Validate(new[]{validator1,validator2},option)
```

> option 是可选参数

#### 验证器初始化器

验证器初始化器通过 `Add` 的各个重载方法来实现。它可以让初始化验证器变得非常灵活。比如向 `Container` 型的验证器（VdObj、And、Or等）中添加子验证器时，可以通过灵活应用这些重载，来随心所欲地初始化验证器。

Add 方法有以下 2 种方式的参数重载：

1. 基方法

   `public void Add(Validator validator, string failureMessage = "")`

   所有重载方法最终都会转换成该接口参数形式调用它

   若路径没有改变，默认路径为 `$`，表示验证的值为当前值

2. `public void Add<T>(T valueOrPath,Validator validator, string failureMessage="")`

   参数分别为：待验证值或路径，验证器，验证失败的消息

其中， T 类型的参数有 4 种形式：

1. 特定值

   表示验证值是一个特定的值，在定义时就确定了，比如 `myClass.Address`

2. 字符串路径

   为了区分字符串路径与字符串值，路径需要添加 `$` 作为前缀 ，在验证时，会根据给定路径动态求解对应的值进行验证。

   其使用方式如： `$Address`

3. 表达式

   第一个参数是一个 lamda 表达式，例如 `()=>myClass.Address`，这种类型会将表达式转换成字符串路径来处理。

Validator 参数也有 3 种形式：

1. Validator

   直接传入一个 Validator 实例

2. 字符串

   程序会自动从 `VlidatorManger` 中按字符串名称查找对应验证器，最后隐式转换成 `Validator` 类型

3. Validator 数组

   程序会将所有 Validator 包装成一个 `And` Validator，然后调用方式 1 进行处理

4. lamda 表达式

   例如 `x=>x>10`。程序会将 lamda 表达式转换成 `Function` 验证器，然后按方式 1 进行处理

通过对上述重载的排列组合，`Add` 方法共有 12 种不同的重载，在实际应用中，可以综合使用，进行非常灵活的初始化。具体使用见 [完整用法示例](#完整用法示例)

### 隐式转换

程序定义了由 string 到 Validator 的隐式转换，可以直接使用 Validator 的 `Name` 来调用 Validator，但这有个缺点，就是不能对参数进行配置，只能使用默认参数。这种适合 `IsNumber`、`IsString` 等的简单验证

同时，程序定义了由 string[] 到 Validator 的显示转换。原理同上，最终会将多个 Validator 包装成 `And`  验证器进行返回。

### 路径

本库的核心是通过路径来定义要验证的值，在运行时动态求解并验证。

#### 定义

路由有两种方式进行定义：

1. 以 `$` 为前缀的字符串形式，例：`$Students[0]`
2. lamda 表达式形式，例：`()=>myClass.Address`

#### 扩展使用

字符串路径有很多丰富的使用方式，在此展开说明下。

以上面 [完整用法示例](#完整用法示例) 中的 `myClass` 为例举例说明

| 路径                            | 描述                                                         |
| ------------------------------- | ------------------------------------------------------------ |
| $                               | 表示当前值，此处为 `myClass`                                 |
| $Name                           | 表示 `myClass.Name`                                          |
| $Address.Email                  | 表示 `myClass.Address.Email`                                 |
| $Students                       | 表示 `myClas.Students`                                       |
| $Students[]                     | 表示 `myClas.Students`                                       |
| $Students[0].Name               | 表示第一个 Student 的 Name                                   |
| $Students[].Name                | 表示所有 Student 的 Name                                     |
| $Students[0].Score[].Key        | 表示第一个 Student 的所有 Score 的 Key。因为 Score 是一个字典类型，因此它的下一级有两个属性，`Key` 和 `Property` |
| $Students[0].Score[].Value.Name | 示第一个 Student 的所有 Score 的 Name。                      |

### 失败消息

失败的消息通过初始化时的第三个参数输入，比如：

``` c#
{ "$Name",new IsString(),"failure message"}
```

对于某些验证器，可以直接在实例化时传入失败的消息，上述代码可以这样使用：

``` c#
{ "$Name",new IsString("failure message")}
```

## 内置验证器

目前程序内置了如下验证器：

| 名称           | 描述                                     | 抽象 | 容器型验证器 |
| -------------- | ---------------------------------------- | ---- | ------------ |
| Validator      | 验证器基类，抽象类                       | Yes  |              |
| Container      | 容器型验证器，抽象类                     | Yes  | Yes          |
| IsString       | 验证是否为字符串                         |      |              |
| IsNumber       | 验证是否为数字                           |      |              |
| inRange        | 验证数字是否在某个范围                   |      |              |
| LessThan       | 验证小于                                 |      |              |
| GreaterThan    | 验证大于                                 |      |              |
| Equals         | 验证是否相等                             |      |              |
| Function       | 用于包裹 lamda 表达式验证器              |      |              |
| NotNullOrEmpty | 验证字符串是否为空                       |      |              |
| Logic          | 逻辑验证器的基类，继承自 Container       |      | Yes          |
| And            | 多个验证器 And 逻辑组合                  |      | Yes          |
| Or             | 多个验证器 Or 逻辑组合                   |      |              |
| VdObj          | 对象验证器容器，继承自 And               |      | Yes          |
| IsArray        | 验证是否是数组，继承自 And               |      | Yes          |
| EachElement    | 列表中的每个元素应用该验证器，继承自 And |      | Yes          |
| ElementAt      | 列表中指定的元素应用该验证器，继承自 And |      | Yes          |

## 容器型验证器

当验证器继承自 `Container` 时，被称之为容器型验证器，容器型验证器中可以增加更多其它的验证器，使用方式示例如下：

``` c#
{ "$Students[0].Scores[].Value.Score",new EachElement(){
    VdNames.IsNumber,
    new GreaterThan(60),
} },
```

## 自定义验证器

当系统内置的验证器无法满足要求时，我们可以通过自定义验证器的方式来实现。

自定义验证器只需要继承 `Validator` 即可。

**注册自定义验证器，实现字符串方式引用**

自定义验证器一般只能通过 `new CustomValidator()` 的方式来使用，如果需要用字符串的方式来引用，需要向 `ValidatorManager` 中注册自定义的验证器，调用如下方法即可：

``` c#
// 添加自定义验证器
ValidatorManager.Instance.AddValidator<CustomValidator>();
```

## 验证器组

`ConfValidator` 支持将一组验证器保存下来，在其它地方进行复用。我们可以提前将验证器组注册到验证器管理器中，然后通过字符串名称来调用。

以 [常用示例](#常用示例) 中 myClass 的验证器组为例：

``` c#
var validatorsGroup = new VdObj() // 顶层是对象类型，使用 VdObj 验证器包裹其它验证器
{
    // 通过验证器名称 IsString 来验证数据 myClass.Name
    { "$Name","IsString"},
    // 逻辑且
    { "$Order",new And(){
        VdNames.IsNumber,
        new LessThan(2),
        new InRange(0,10),
        new Function<int>(x=>x>1)
    } }, 
    { ()=>myClass.Address,new VdObj{
        { "$Email",new IsString()}
    } } 
});

// 注册
// 第一个参数必须是名称，且不能与已有的验证器重复
ValidatorManager.Instance.AddValidator("validateMyClass",validatorsGroup);

// 使用
myClass.Validate("validateMyClass");
```

## 赞助与支持

如果觉得插件不错，可以请作者喝一杯咖啡哟！

<div style="display:flex;justify-content:space-around;">
<img height="200px" src="https://i.loli.net/2021/08/13/JOw9cxomhBAZFW8.png" alt="wechat">
<img height="200px" src="https://i.loli.net/2021/08/13/U2s7gKn1zRw3uP4.png" alt="ailipay">
<div />