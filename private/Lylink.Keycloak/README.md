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
    - The secret to be used for the blog site application to authenticate with Keycloak as `BLOG_SITE_CLIENT_SECRET`.
    - The expected password for Keycloak to read/write to the Postgres database.
    - The expected password for the bootstrapped Keycloak admin account.

> **Important** - The `realm.lylink.template.json` file is the canonical source of truth for the Lylink realm. Aside from the secrets substituted into it at container startup, the realm configuration in the repository is authoritative - it is re-imported on every container start, so any changes made by hand through the Keycloak admin UI will not persist across restarts.

## Image Build

The `Dockerfile` copies `realm.lylink.template.json` into the image unresolved, along with [`initialize-keycloak.sh`](./initialize-keycloak.sh), which is set as the image's entrypoint. Hydration of the template no longer happens before the image is built - it happens at container startup, using whatever environment variables are present on the running container.

This matters because the built image is published to a shared registry: baking a hydrated `realm.lylink.json` into an image layer would mean secret values (client secrets, admin passwords) end up stored in the registry itself. Keeping the template unresolved in the image and substituting values only at runtime keeps the published image environment-agnostic and free of secrets.

`initialize-keycloak.sh` is responsible for:

1. Substituing the environment-specific variables against `realm.lylink.template.json` to produce `realm.lylink.json` inside the running container, using the environment variables provided to the container via `docker-compose`'s `environment:` and `env_file:` configuration.
2. Importing the new `realm.lylink.json` into Keycloak.
2. Handing off to `kc.sh start --optimized --http-port 7080` to launch Keycloak with the freshly-hydrated realm.

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

With a `.env` file in place, both the Keycloak image and its Postgres database dependency can now be launched with the [`docker-compose.yml`](./docker-compose.yml):

```powershell
docker compose up -d;
```

`docker-compose.yml` passes the values from `.env` into the `keycloak` container's environment, where `initialize-keycloak.sh` substitutes them into `realm.lylink.template.json` and imports the result on startup. To access the running instance via the web UI, please log in via the configured `KEYCLOAK_BOOTSTRAP_ADMIN_PASSWORD` password under the `bootstrap-admin` account.
