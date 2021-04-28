
docker run --rm  -v  52abp-gitlab-ci-config:/etc/gitlab-runner gitlab/gitlab-runner register \
  --url "http://code.52abp.com/" \
  --registration-token "rcr6LVEQzsJ3fRYakQgP" \
  --non-interactive \
  --executor "docker" \
  --docker-image docker:19.03.12 \
  --docker-privileged="true" \
  --docker-volumes "/certs/client" \
  --description "docker-runner" \
  --tag-list "angular" \
  --run-untagged="true" \
  --locked="false" \
  --access-level="not_protected"


docker run --rm  -v  52abp-gitlab-ci-config:/etc/gitlab-runner gitlab/gitlab-runner register \
  --url "http://code.52abp.com/" \
  --registration-token "rcr6LVEQzsJ3fRYakQgP" \
  --non-interactive \
  --executor "docker" \
  --docker-image docker:19.03.12 \
  --docker-privileged="true" \
  --docker-volumes "/certs/client" \
  --description "docker-runner" \
  --tag-list "vue" \
  --run-untagged="true" \
  --locked="false" \
  --access-level="not_protected"

docker run --rm  -v  52abp-gitlab-ci-config:/etc/gitlab-runner gitlab/gitlab-runner register \
  --url "http://code.52abp.com/" \
  --registration-token "rcr6LVEQzsJ3fRYakQgP" \
  --non-interactive \
  --executor "docker" \
  --docker-image docker:19.03.12 \
  --docker-privileged="true" \
  --docker-volumes "/certs/client" \
  --description "docker-runner" \
  --tag-list "dotnet" \
  --run-untagged="true" \
  --locked="false" \
  --access-level="not_protected"


docker run --rm  -v  52abp-gitlab-ci-config:/etc/gitlab-runner gitlab/gitlab-runner register \
  --url "http://code.52abp.com/" \
  --registration-token "rcr6LVEQzsJ3fRYakQgP" \
  --non-interactive \
  --executor "docker" \
  --docker-image docker:19.03.12 \
  --docker-privileged="true" \
  --docker-volumes "/certs/client" \
  --description "docker-runner" \
  --tag-list "nuget" \
  --run-untagged="true" \
  --locked="false" \
  --access-level="not_protected"


  docker run --rm  -v  52abp-gitlab-ci-config:/etc/gitlab-runner gitlab/gitlab-runner register \
  --url "http://code.52abp.com/" \
  --registration-token "rcr6LVEQzsJ3fRYakQgP" \
  --non-interactive \
  --executor "docker" \
  --docker-image docker:19.03.12 \
  --docker-privileged="true" \
  --docker-volumes "/certs/client" \
  --description "docker-runner" \
  --tag-list "docker" \
  --run-untagged="true" \
  --locked="false" \
  --access-level="not_protected"