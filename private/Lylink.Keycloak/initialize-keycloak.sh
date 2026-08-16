#!/bin/sh
set -e
sed \
  -e "s|\${MANAGEMENT_SITE_ROOT_URL}|$MANAGEMENT_SITE_ROOT_URL|g" \
  -e "s|\${MANAGEMENT_SITE_ADMIN_URL}|$MANAGEMENT_SITE_ADMIN_URL|g" \
  -e "s|\${MANAGEMENT_SITE_CLIENT_SECRET}|$MANAGEMENT_SITE_CLIENT_SECRET|g" \
  -e "s|\${BLOG_SITE_CLIENT_SECRET}|$BLOG_SITE_CLIENT_SECRET|g" \
  /tmp/realm.lylink.template.json > /tmp/realm.lylink.json
/opt/keycloak/bin/kc.sh import --file /tmp/realm.lylink.json --override true
exec /opt/keycloak/bin/kc.sh start --optimized --http-port 7080
