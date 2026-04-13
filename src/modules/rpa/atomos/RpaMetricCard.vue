<template>
  <v-card class="metric-card" :class="`metric-${variant}`" elevation="0">
    <div class="metric-card__accent" />
    <div class="metric-card__content">
      <div class="metric-card__left">
        <div class="metric-card__label">{{ label }}</div>
        <div class="metric-card__value">{{ value }}</div>
      </div>
      <div class="metric-card__right">
        <div class="metric-card__icon-wrap">
          <v-icon size="22">{{ icon }}</v-icon>
        </div>
      </div>
    </div>
  </v-card>
</template>

<script setup lang="ts">
type MetricVariant = 'primary' | 'teal' | 'green' | 'red';

interface Props {
  value: string | number;
  label: string;
  icon: string;
  variant: MetricVariant;
}

defineProps<Props>();
</script>

<style scoped>
.metric-card {
  border-radius: 8px;
  overflow: hidden;
  transition:
    transform 0.25s ease,
    box-shadow 0.25s ease;
  border: 1px solid rgba(0, 0, 0, 0.08);
}

.metric-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 22px rgba(16, 24, 40, 0.12) !important;
}

.metric-card__accent {
  height: 4px;
}

.metric-card__content {
  padding: 14px 16px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
}

.metric-card__value {
  margin-top: 6px;
  font-size: 28px;
  font-weight: 900;
  line-height: 1.05;
  color: #111827;
}

.metric-card__label {
  font-size: 12px;
  font-weight: 700;
  color: #6b7280;
  letter-spacing: 0.02em;
}

.metric-card__icon-wrap {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  display: grid;
  place-items: center;
}

.metric-primary .metric-card__accent {
  background: #1c8dce;
}
.metric-primary .metric-card__icon-wrap {
  color: #1c8dce;
  background: rgba(28, 141, 206, 0.1);
}

.metric-teal .metric-card__accent {
  background: #1aa6b7;
}
.metric-teal .metric-card__icon-wrap {
  color: #1aa6b7;
  background: rgba(26, 166, 183, 0.1);
}

.metric-green .metric-card__accent {
  background: #2e7d32;
}
.metric-green .metric-card__icon-wrap {
  color: #2e7d32;
  background: rgba(46, 125, 50, 0.1);
}

.metric-red .metric-card__accent {
  background: #d32f2f;
}
.metric-red .metric-card__icon-wrap {
  color: #d32f2f;
  background: rgba(211, 47, 47, 0.1);
}
</style>
