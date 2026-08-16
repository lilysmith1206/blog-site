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

# Copies the relevant files to initialize the Lylink Keycloak realm.
COPY --chmod=0755 initialize-keycloak.sh /opt/keycloak/initialize-keycloak.sh

RUN /opt/keycloak/bin/kc.sh build --health-enabled=true

FROM quay.io/keycloak/keycloak:latest
COPY --from=builder /opt/keycloak/ /opt/keycloak/
COPY realm.lylink.template.json /tmp/realm.lylink.template.json

ENV KC_HTTP_ENABLED=true
ENV KC_PROXY_HEADERS=xforwarded
ENV KC_HOSTNAME_STRICT=false
ENV KC_HEALTH_ENABLED=true
ENV KC_METRICS_ENABLED=true
ENV KC_HOSTNAME_STRICT_BACKCHANNEL=false
ENV KC_HOSTNAME_BACKCHANNEL_DYNAMIC=true
ENV KC_LOG_LEVEL=info
ENV KC_DB=postgres

ENTRYPOINT ["/opt/keycloak/initialize-keycloak.sh"]