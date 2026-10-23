# LeadLane

Mini CRM for inbound leads: a public callback form → kanban board → notes and next contact.

Built as a small, runnable ASP.NET Core portfolio product for freelancers, studios, and local businesses that currently track “please call me” requests in Telegram or Excel.

## What it does

1. **Public form** — visitors leave name, phone, optional email, and a message.
2. **Admin login** — cookie auth; leads are not public.
3. **Kanban** — columns by status; change status from the board or the lead card.
4. **Lead card** — contact fields, notes, status, and next-contact date.
5. **Needs attention** — overdue next-contact leads surface on the board.
6. **Search** — find leads by name or phone.

### Pipeline statuses

`New` → `In progress` → `Agreed` → `Won` | `Lost`

## Stack

- ASP.NET Core 10 (MVC, Razor)
- Custom CSS + light JS
- PostgreSQL + EF Core
- Layers: `LeadLane.Domain` / `LeadLane.Infrastructure` / `LeadLane.Web`

## Quick start

### 1. Database

```bash
cp .env.example .env
docker compose up -d
```

### 2. Connection string

```bash
cp LeadLane.Web/appsettings.Development.local.json.example LeadLane.Web/appsettings.Development.local.json
```

Align the password with `.env` / `docker-compose.yml` if you changed it.

### 3. Run

```bash
dotnet run --project LeadLane.Web
```

Open the URL from the console (default: `https://localhost:7144`).

On startup the app migrates the DB and seeds a demo admin plus sample leads.

## Demo login

| Field    | Value      |
|----------|------------|
| Login    | `admin`    |
| Password | `admin123` |

## Try the flow

1. Open the home page — read what LeadLane is, submit a callback request.
2. Sign in via **Go to Admin Panel** (`admin` / `admin123`).
3. See the new lead in **New**, move it with the status control.
4. Open the lead card — add notes and set a next contact.
5. Set next contact in the past (or use the seeded overdue lead) — it appears under **Needs attention**.
6. Use search by name or phone.

## Project layout

```
LeadLane/
  LeadLane.Domain/          # Lead, User, services, interfaces
  LeadLane.Infrastructure/  # EF, repositories, seed
  LeadLane.Web/             # public form, login, board, lead card
  README.md
```

## Out of scope (by design)

No roles/permissions UI, contacts module, email/SMS, payments, scheduling, or multi-tenant setup. One demo admin is enough for the MVP.
