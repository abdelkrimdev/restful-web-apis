#!/bin/bash

mongo <<EOF
use $TODO_MONGO_DB
db.createUser({
  user: '$TODO_MONGO_USER',
  pwd:  '$TODO_MONGO_PASS',
  roles: ['dbOwner']
})
EOF
