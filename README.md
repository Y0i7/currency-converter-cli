# 💱 Currency Converter CLI

A simple and powerful **console application built with C# and .NET** that allows users to convert currencies in real time using an external API.

---

## 🚀 Features

* Convert currencies using real-time exchange rates
* List supported currencies
* Interactive CLI mode
* Clean architecture (Services, Clients, Models)
* External configuration using `appsettings.json`

---

## 🛠️ Tools & Technologies

| Tool / Technology | Version                                      |
| ----------------- | -------------------------------------------- |
| C#                | Latest (C# 12+)                              |
| .NET SDK          | .NET 10                                      |
| IDE               | JetBrains Rider / VS Code                    |
| HTTP Client       | System.Net.Http                              |
| Configuration     | Microsoft.Extensions.Configuration (v10.0.5) |
| JSON Parsing      | System.Text.Json                             |

---

## 🌐 API Used

This project uses the **Frankfurter API**:

* Base URL:

```text
https://api.frankfurter.dev/v1/
```

* Features:

  * Free and open-source
  * No API key required
  * Based on European Central Bank data
  * Daily updated exchange rates

---

## ⚙️ Configuration

The application uses an external configuration file:

### `appsettings.json`

```json
{
  "ApiSettings": {
    "BaseUrl": "https://api.frankfurter.dev/v1/"
  }
}
```

---

## 📦 Installation

Clone the repository:

```bash
git clone https://github.com/Y0i7/currency-converter-cli.git
cd currency-converter-cli
```

Restore dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run
```

---

## 💻 Usage

### 🔹 Convert currencies

```bash
dotnet run -- convert USD EUR 100
```

**Example Output:**

```text
100 USD = 92 EUR (rate 0.92)
```

---

### 🔹 List supported currencies

```bash
dotnet run -- list
```

**Example Output:**

```text
AUD, BRL, CAD, CHF, COP, EUR, GBP, JPY, USD
```

---

### 🔹 Help command

```bash
dotnet run -- help
```

**Output:**

```text
Commands:
  convert <FROM> <TO> <AMOUNT>
  list
  (no args) interactive mode
```

---

### 🔹 Interactive mode

Run without arguments:

```bash
dotnet run
```

Then type:

```text
> convert USD COP 10
> list
> help
> exit
```

---

## 🧠 Architecture Overview

```
Program.cs        → Application entry point
Services/         → Business logic
Clients/          → External API communication
Models/           → Data structures
Utils/            → CLI helper logic
```

---

## ⚠️ Error Handling

* Invalid amount input is validated
* API errors are handled with exceptions
* Invalid commands show help message

---

## 📌 Notes

* Exchange rates are fetched in real time
* Uses `decimal` for financial precision
* Designed with scalability in mind (DI-ready)

---

## 👨‍💻 Author

Developed by Yoi 🐯
Aspiring Software Engineer passionate about backend and architecture.
---
