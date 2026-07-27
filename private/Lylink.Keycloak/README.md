# Keycloak Infrastructure

This is a folder containing configuration files to set up a Keycloak instance for the Lylink applications. It contains:

- A realm configuration file for the `lylink` realm, with clients for:
    - The blog site UI application.
    - The management site UI application.
    - Permissions to perform various operations:
        - Creating visitor analytics.
- A `.env.template` file containing the following:
    - The expected root URL of the management site as `MANAGEMENT_SITE_ROOT_URL`.
    - The expected admin URL of the management site as `MANAGEMENT_SITE_ADMIN_URL`.
    - The secret to be used for the management site application to authenticate with Keycloak as `MANAGEMENT_SITE_CLIENT_SECRET`.
    - The secret to be used for the blog site applicawtion to authenticate with Keycloak as `BLOG_SITE_CLIENT_SECRET`.
    - The expected password for Keycloak to read/write to the Postgres database.
    - The expected password for the bootstrapped Keycloak admin account.

> **Important** - This guide requires either a Linux development environment or a [Windows Subsystem for Linux](https://learn.microsoft.com/en-us/windows/wsl/about) (WSL) component on a Windows machine to complete the setup. Please ensure that the local system meets either requirement before proceeding.

## Configuration Instructions

This section will go over the instructions to deploy and configure the Keycloak realm. 

It is expected that a file `.env` will be created with the following format:

```
MANAGEMENT_SITE_ROOT_URL=https://management-site.io
MANAGEMENT_SITE_ADMIN_URL=https://management-site.io
MANAGEMENT_SITE_CLIENT_SECRET=<secret value>
BLOG_SITE_CLIENT_SECRET=<secret value>
KEYCLOAK_POSTGRES_PASSWORD=<password>
KEYCLOAK_BOOTSTRAP_ADMIN_PASSWORD=<password>
```

from [.env.template](./.env.template).

> **Important** - The `.env` file created for this process must be ignored by git, as it contains sensitive information. It should be ignored by default, but double-check this if running this process locally.

The `realm.lylink.template.json` is a template realm file containing all of the clients, users, and permissions needed for the rest of the system work as expected. In order to create a hydrated version of the realm information from the .env information, please use the below commands:

```bash
# ensure that you're in `/private/Lylink.Keycloak/` for these steps.

# sets the terminal to recognize all values that are imported through source to be set to the terminal's environment.
set -a

# imports all environment variables from the .env file configured in the previous step.
source .env

# disables the `set -a` so terminal commands can be ran normally.
set +a

# substitues all variables in the template with values from the .env file read into the environment
# then exports the new file to realm.lylink.json for use with the Lylink Keycloak image.
envsubst < realm.lylink.template.json > realm.lylink.json
```

With the `realm.lylink.json` created, both the Keycloak image and its Postgres database dependency can now be launched with the [`docker-compose.yml`](./docker-compose.yml):

```powershell
docker compose up -d;
```

This will build and run the Keycloak instance configured with the Lylink realm data. To access it via the web UI, please log in via the configured `KEYCLOAK_BOOTSTRAP_ADMIN_PASSWORD` password under the `bootstrap-admin` account.