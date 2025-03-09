docker-build:
  docker build -t parkour-backend:latest .

build-and-deploy:
  #!/bin/bash

  just docker-build
  docker save parkour-backend:latest | bzip2 > /tmp/parkour-backend.tar.bz2
  scp /tmp/parkour-backend.tar.bz2 debian@ameo.dev:/tmp/parkour-backend.tar.bz2
  ssh debian@ameo.dev -t "cat /tmp/parkour-backend.tar.bz2 | bunzip2 | docker load && docker kill parkour-backend && docker container rm parkour-backend && docker run -d --name parkour-backend -p 5800:8080 --restart always -v /opt/parkour/db:/app/db parkour-backend:latest && rm /tmp/parkour-backend.tar.bz2" && rm /tmp/parkour-backend.tar.bz2
