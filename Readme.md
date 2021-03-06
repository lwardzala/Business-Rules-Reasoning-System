# Business Rules Reasoning System

Cross-Platform Business Rules Reasoning System. Is a simple reasoning tool based on implication form of the Horn clause.
Provides integration between various systems and supports workflows.

Automate decision processes with the Business Rules Reasoning System!

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](License)

Actually the Reasoning System contains:
- Reasoning.Core - A library that provides knowledge base builders and base reasoning service. Can be used independently in any .NET project;
- Reasoning.Host - The biggest part that provides reasoning task management and integration.

## Features
- Reasoning can be stopped in any time and resumed after collecting more facts;
- Two reasoning methods: Deduction and Hypothesis Testing;
- No need to fill all facts (variables) to finish reasoning. Reasoning finishes when entropy is zero;
- ReasoningResolver tries to obtain facts (variables) automatically by requesting to defined APIs;
- ReasoningResolver executes requests to other APIs depend on the reasoning result (support for workflows);
- Reasoning.Host can be runned on Windows/Linux/macOS;
- MongoDB integration.

If you are still considering how would you utilize a reasoning system, take a look on [Use cases](#use-cases) part.

## Table of content

- [Business Rules Reasoning System](#business-rules-reasoning-system)
  - [Features](#features)
  - [Table of content](#table-of-content)
  - [Reasoning.Core](#reasoningcore)
    - [Installation (NuGet)](#installation-nuget)
    - [Predicates](#predicates)
    - [Rules](#rules)
    - [Knowledge Base](#knowledge-base)
    - [Variables and supported values](#variables-and-supported-values)
    - [Supported operators](#supported-operators)
    - [Reasoning Process](#reasoning-process)
    - [Reasoning Method](#reasoning-method)
    - [Reasoning Service](#reasoning-service)
  - [Reasoning.Host](#reasoninghost)
    - [Installation](#installation)
    - [REST API](#rest-api)
    - [Knowledge Base](#knowledge-base-1)
    - [Reasoning Task](#reasoning-task)
  - [Use cases](#use-cases)
  - [Future functionalities](#future-functionalities)
    - [Coming soon](#coming-soon)
    - [Long term plans](#long-term-plans)
  - [Authors](#authors)
  - [References](#references)

## Reasoning.Core

Reasoning.Core library (netstandard2.0) is the engine of the Reasoning System. It's used inside of Reasoning.Host and able to be used in an external project as a bridge between your app and the Host or can be used independently to optimize your app.

### Installation (NuGet)

https://www.nuget.org/packages/Reasoning.Core/

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Reasoning.Core)

### Predicates

Predicate is the smallest structure and represents equation.
They are built from: LeftTerm, Operator and RightTerm.

So representation of an equation:
```
age >= 18
```

Will be:
```json
{
  "leftTerm": {
    "id": "age",
    "name": "Age",
    "value": null
  },
  "operator": "GreaterOrEqual",
  "rightTerm": {
    "id": "age",
    "name": "Age",
    "value": 18
  }
}
```

With PredicateBuilder:
```csharp
using Reasoning.Core;
using Reasoning.Core.Contracts;

new PredicateBuilder()
    .ConfigurePredicate("age", OperatorType.GreaterOrEqual, 18)
    .SetLeftTermValue(23) // optional for left term
    .Unwrap()
```

### Rules

Rule is the middle structure in Knowledge Base. Each rule has a set of predicates and a conclusion.
A rule is being evaluated during reasoning process by evaluating it's predicates. If any predicate's evaluation result is false, then the rule's evaluation result is false.

So representation of rule:
```
age >= 18 -> passenger = 'adult'
```

Will be:
```json
{
  "predicates": [
    {
      "leftTerm": {
        "id": "age",
        "name": "Age",
        "value": null
      },
      "operator": "GreaterOrEqual",
      "rightTerm": {
        "id": "age",
        "name": "Age",
        "value": 18
      }
    }
  ],
  "conclusion": {
    "id": "passenger",
    "name": "Passenger type",
    "value": "adult"
  }
}
```

With RuleBuilder:
```csharp
using Reasoning.Core;
using Reasoning.Core.Contracts;

new RuleBuilder()
    .SetConclusion(new VariableBuilder()
        .SetId("passenger")
        .SetName("Passenger type")
        .SetValue("adult") // required for conclusion
        .Unwrap())
    .AddPredicate(new PredicateBuilder()
        .ConfigurePredicate("age", OperatorType.GreaterOrEqual, 18)
        .SetLeftTermValue(23) // optional for left term
        .Unwrap())
    .Unwrap()
```

### Knowledge Base

Knowledge base is the biggest unit and it's a representation of a use case. Contains Id, Name, Description and Rule Set.
The base can be modified or involved in any time by an 'expert' to improve business process.

Sample of knowledge base rule set:
```
age >= 18 -> passenger = "adult"
age < 18 & age >= 5 -> passenger = "child"
age < 5 -> passenger = "toddler"
```


<details><summary>Json representation</summary>
<p>

```json
[
  {
    "predicates": [
      {
        "leftTerm": {
          "id": "age"
        },
        "operator": "GreaterOrEqual",
        "rightTerm": {
          "id": "age",
          "value": 18
        }
      }
    ],
    "conclusion": {
      "id": "passenger",
      "value": "adult"
    }
  },
  {
    "predicates": [
      {
        "leftTerm": {
          "id": "age"
        },
        "operator": "LesserThan",
        "rightTerm": {
          "id": "age",
          "value": 18
        }
      },
      {
        "leftTerm": {
          "id": "age"
        },
        "operator": "GreaterOrEqual",
        "rightTerm": {
          "id": "age",
          "value": 5
        }
      }
    ],
    "conclusion": {
      "id": "passenger",
      "value": "child"
    }
  },
  {
    "predicates": [
      {
        "leftTerm": {
          "id": "age"
        },
        "operator": "LesserThan",
        "rightTerm": {
          "id": "age",
          "value": 5
        }
      }
    ],
    "conclusion": {
      "id": "passenger",
      "value": "toddler"
    }
  }
]
```

</p>
</details>

With KnowledgeBaseBuilder:
```csharp
new KnowledgeBaseBuilder()
    .SetId("knowledgeBase1")
    .SetName("Knowledge Base 1")
    .SetDescription("Passengers type principles")
    .AddRule(new RuleBuilder()
        .SetConclusion(new VariableBuilder()
            .SetId("passenger")
            .SetValue("adult")
            .Unwrap())
        .AddPredicate(new PredicateBuilder()
            .ConfigurePredicate("age", OperatorType.GeaterOrEqual, 18)
            .Unwrap())
        .Unwrap())
    .AddRule(new RuleBuilder()
        .SetConclusion(new VariableBuilder()
            .SetId("passenger")
            .SetValue("child")
            .Unwrap())
        .AddPredicate(new PredicateBuilder()
            .ConfigurePredicate("age", OperatorType.LesserThan, 18)
            .Unwrap())
        .AddPredicate(new PredicateBuilder()
            .ConfigurePredicate("age", OperatorType.GreaterOrEqual, 5)
            .Unwrap())
        .Unwrap())
    .AddRule(new RuleBuilder()
        .SetConclusion(new VariableBuilder()
            .SetId("passenger")
            .SetValue("toddler")
            .Unwrap())
        .AddPredicate(new PredicateBuilder()
            .ConfigurePredicate("age", OperatorType.LesserThan, 5)
            .Unwrap())
        .Unwrap())
    .Unwrap();
```

### Variables and supported values

Variables are the smallest units. Both Terms and Conclusions are instances of a Variable type.

Each Variable has a value. The value is not strongly typed and can be a:

- string;
- number;
- boolean;
- array.

Value types are comparable with each other.

### Supported operators

<u>Available operators:</u>

- Equal;
- NotEqual;
- GreaterThan;
- LesserThan;
- GreaterOrEqual;
- LesserOrEqual;
- IsIn (invertion of Contains);
- NotIn (negation of IsIn);
- Between (ex. "4 Between [3, 5]" is true);
- NotBetween (negation of Between);
- Subset (Cross type, ex. "4 subset [3, 4, 7]" is true, "[3, 4] subset [3, 4, 7]" is true);
- NotSubset (invertion of Subset).

### Reasoning Process

Reasoning process is a state of reasoning. It contains all necessary data for reasoning like: Reasoning Method, Knowledge Base, State, Reasoned Items, Evaluation Message and Hypothesis.

For initializing Reasoning Process use ReasoningProcessFactory:
```csharp
using Reasoning.Core;

ReasoningProcessFactory.CreateInstance(knowledgeBase, ReasoningMethod.HypothesisTesting, hypothesis);
```

<u>Possible State:</u>

- INITIALIZED;
- STARTED;
- STOPPED;
- FINISHED.

<u>Possible Evaluation Message:</u>

- NONE - initialized or started;
- PASSED - at least one rule is true or hypothesis is confirmed;
- FAILED - all rules are false or hypothesis is not confirmed;
- ERROR - an error occured while reasoning;
- MISSING_VALUES - there are no enough facts to finish reasoning.

When a rule or hypothesis is true, it's conclusion is being added to Reasoned Items parameter.

### Reasoning Method

<u>There are two reasoning modes:</u>

- Deduction - tries to reason as many conclusions as possible;
- HypothesisTesting - checks if any rule can confirm the hypothesis (any rule that is true and it's conclusion is same as the hypothesis).

### Reasoning Service

A service that contains all procedures for reasoning:

- StartReasoning - starts reasoning with reseting all evaluation states and left term values;
- ContinueReasoning - continues reasoning with current state (without reset);
- SetValues - tries to set appropriate variables to every rule;
- ResetReasoning - resets reasoning evaluation but preserving left term values;
- ClearReasoning - resets reasoning evaluation and variables;
- GetAllMissingVariableIds - tries to get all missing variable ids in reasoning process.

## Reasoning.Host

It's a REST API (netcoreapp3.1) that provides reasoning tasks manager and the integration feature.
The Host enqueues provided Reasoning Tasks and tries to resolve them step by step.

Every task is being resolved asynchronously by Background Worker. It's possible to check the current reasoning state only by requesting the detail by Reasoning Task Id.

### Installation

1. Install .NET Core 3.1
   
To run the Host application, it's needed to install .NET Core SDK (for project compilation) or .NET Core Runtime (for running compiled app on a server)
https://dotnet.microsoft.com/download/dotnet-core/3.1

2. Configure database connection string and app address
   
Put MongoDB connection string in Source/Reasoning.Host/appsettings.json

appsettings.json
```json
{
  "MongoDatabaseSettings": {
    "KnowledgeBaseCollectionName": "knowledge_base",
    "ReasoningTaskCollectionName": "reasoning_tasks",
    "VariableSourceCollectionName": "variable_sources",
    "VariableCollectionName": "variables",
    "ConnectionString": "put-your-mongodb-url-here", // Your MongoDB connection string
    "DatabaseName": "reasoning"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Urls": "https://0.0.0.0:5001" // Configure the application address
}
```

3. Compile and run the app
   
Run development mode bu using
```
dotnet run
```

Or build a production package
```
dotnet publish
```

### REST API
For more see: [Postman Collection](PostmanCollection.json)

### Knowledge Base

The Host app stores all provided Knowledge Bases. It's possible to add, update and delete them from database.

<u>To get Knowledge Base use:</u>
- GET /api/knowledge-base/{id}

<u>To add a Knowledge Base use:</u>
- <p>POST /api/knowledge-base/
<details><summary>Sample body</summary>
<p>

```json
{
    "knowledgeBase": {
        "id": "knowledgeBase6",
        "name": "Knowledge Base 1",
        "description": "Testing reasoning service",
        "ruleSet": [
            {
                "conclusion": {
                    "id": "conclusion1",
                    "value": "Conclusion 1"
                },
                "predicates": [
                    {
                        "leftTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 3
                        },
                        "rightTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 3
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "rightTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1",
                                "opt2"
                            ]
                        },
                        "rightTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1",
                                "opt2"
                            ]
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    }
                ],
                "result": false,
                "evaluated": false
            },
            {
                "conclusion": {
                    "id": "conclusion1",
                    "value": "Conclusion 1"
                },
                "predicates": [
                    {
                        "leftTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 3
                        },
                        "rightTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 4
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "rightTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1"
                            ]
                        },
                        "rightTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1"
                            ]
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    }
                ],
                "result": false,
                "evaluated": false
            }
        ]
    }
}
```

</p>
</details></p>

<u>To update Knowledge Base use:</u>
- <p>PUT /api/knowledge-base/{id}
<details><summary>Sample body</summary>
<p>

```json
{
    "knowledgeBase": {
        "id": "knowledgeBase2",
        "name": "Knowledge Base 1 update 2",
        "description": "Testing reasoning service",
        "ruleSet": [
            {
                "conclusion": {
                    "id": "conclusion1",
                    "name": "Conclusion 1"
                },
                "predicates": [
                    {
                        "leftTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 3
                        },
                        "rightTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 3
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "rightTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1",
                                "opt2"
                            ]
                        },
                        "rightTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1",
                                "opt2"
                            ]
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    }
                ],
                "result": false,
                "evaluated": false
            },
            {
                "conclusion": {
                    "id": "conclusion1",
                    "name": "Conclusion 1"
                },
                "predicates": [
                    {
                        "leftTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 3
                        },
                        "rightTerm": {
                            "id": "var1",
                            "name": "var1",
                            "value": 4
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "rightTerm": {
                            "id": "var2",
                            "name": "var2",
                            "value": "OK"
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    },
                    {
                        "leftTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1"
                            ]
                        },
                        "rightTerm": {
                            "id": "var3",
                            "name": "var3",
                            "value": [
                                "opt1"
                            ]
                        },
                        "operator": "Equal",
                        "result": false,
                        "evaluated": false
                    }
                ],
                "result": false,
                "evaluated": false
            }
        ]
    }
}
```

</p>
</details></p>

<u>To delete Knowledge Base use:</u>
- DELETE /api/knowledge-base/{id}

### Reasoning Task

Reasoning Tasks relate to an existing Knowledge Base by Id. Also there is a possibility to provide:
- Sources - a collection of variable source objects that contain variable ids that can be obtained and a request pattern object;
- Actions - a collection of request patterns and reasoned items (conclusions) that indicate when the requests should be executed.

<u>To get Reasoning Task use:</u>
- GET /api/reasoning-task/{id}

<u>To get detailed Reasoning Task use:</u>
- GET /api/reasoning-task/{id}/detail

<u>To add Reasoning Task use:</u>
- <p>POST /api/reasoning-task/
<details><summary>Sample body</summary>
<p>

```json
{
    "knowledgeBaseId": "knowledgeBase5",
    "reasoningMethod": "Deduction",
    "description": "Test reasoning task",
    "sources": [
        {
            "variableIds": [ "var1", "var2", "var3" ],
            "request": {
                "method": "GET",
                "uri": "https://localhost:44384/some-resource"
            }
        }
    ]
}
```

</p>
</details></p>

<u>To manually set variables use:</u>
- <p>PUT /api/reasoning-task/{id}/variables (after providing variables, system tries to resume reasoning automatically)
<details><summary>Sample body</summary>
<p>

```json
{
    "variables": [
        {
            "id": "var1",
            "name": "var1",
            "value": 3
        },
        {
            "id": "var2",
            "name": "var2",
            "value": "OK"
        },
        {
            "id": "var3",
            "name": "var3",
            "value": [
                "opt1",
                "opt2"
            ]
        }
    ]
}
```

</p>
</details></p>

<u>To delete Reasoning Task use:</u>
- DELETE /api/reasoning-task/{id}

## Use cases

The Reasoning System can be used whenever you want to automate a decision process:

1. You have a production hall and want to automate production process. You can connect the Host application with production machine's API to avoid any to be idle;

2. You have a complex screening process when customer requests for a loan. There is a need to collect data from various anti-fraud databases and decide whether grant a loan or not;

3. You have a process with several workflows to be executed. The next workflow depends on result of the previous one;

4. You are a software developer and want to improve code of a service that contains multi-level "if" clauses. 

## Future functionalities
### Coming soon
- Shared variable collection (reasoning results and predicate variables can be shared between different reasoning tasks)
- Integration with PostgreSQL/MS SQL databases;
- Swagger;

### Long term plans
- JavaScript library of the Reasoning.Core;
- Graphic interface for composing and modifying knowledge bases;
- Scalability;

## Authors
- Lukasz Wardzala - [github](https://github.com/lwardzala)

## References
- [Horn clause wiki](https://en.wikipedia.org/wiki/Horn_clause)