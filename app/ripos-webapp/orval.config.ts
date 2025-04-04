import { defineConfig } from 'orval';

export default defineConfig({
  api: {
    output: {
      target: 'src/api/generated/api.ts',
      schemas: 'src/api/generated/models',
      client: 'react-query',
      override: {
        mutator: 'src/api/axiosMutator.ts',
      },
      clean: true,
      mode: 'tags-split',
    },
    input: 'src/api/swagger.json',
    hooks: {
      afterAllFilesWrite: 'prettier --write',
    },
  },
});
