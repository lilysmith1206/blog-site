FROM quay.io/keycloak/keycloak:26.0 AS builder

ENV KC_HTTP_ENABLED=true
ENV KC_PROXY_HEADERS=xforwarded
ENV KC_HOSTNAME_STRICT=false
ENV KC_HEALTH_ENABLED=true
ENV KC_METRICS_ENABLED=true
ENV KC_HOSTNAME_STRICT_BACKCHANNEL=false
ENV KC_HOSTNAME_BACKCHANNEL_DYNAMIC=true
ENV KC_LOG_LEVEL=info
ENV KC_DB=postgres


# Copies all of realm configuration files for use to import on docker run.
COPY realm.lylink.json /opt/keycloak/data/import/realm.lylink.json

RUN /opt/keycloak/bin/kc.sh build --health-enabled=true

FROM quay.io/keycloak/keycloak:latest
COPY --from=builder /opt/keycloak/ /opt/keycloak/

ENV KC_HTTP_ENABLED=true
ENV KC_PROXY_HEADERS=xforwarded
ENV KC_HOSTNAME_STRICT=false
ENV KC_HEALTH_ENABLED=true
ENV KC_METRICS_ENABLED=true
ENV KC_HOSTNAME_STRICT_BACKCHANNEL=false
ENV KC_HOSTNAME_BACKCHANNEL_DYNAMIC=true
ENV KC_LOG_LEVEL=info
ENV KC_DB=postgres

ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start",  "--import-realm", "--optimized", "--http-port", "7080"]