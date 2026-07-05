import { test, expect } from '@playwright/test';

test('1. Homepage loads successfully', async ({ page }) => {
  await page.goto('/');
  await expect(page).toHaveTitle(/Vjezba|TEH|Oprema/i);
  await expect(page.locator('body')).toBeVisible();
});

test('2. Navigation menu is visible', async ({ page }) => {
  await page.goto('/');
  const nav = page.locator('nav, .sidebar, .navbar, .side-nav');
  await expect(nav.first()).toBeVisible();
});

test('3. RadnaOprema list page loads', async ({ page }) => {
  await page.goto('/oprema');
  await expect(page).toHaveURL(/oprema/);
  await expect(page.locator('body')).toBeVisible();
});

test('4. Radnik list page loads', async ({ page }) => {
  await page.goto('/radnici');
  await expect(page).toHaveURL(/radnici/);
  await expect(page.locator('body')).toBeVisible();
});

test('5. Lokacija list page loads', async ({ page }) => {
  await page.goto('/lokacije');
  await expect(page).toHaveURL(/lokacije/);
  await expect(page.locator('body')).toBeVisible();
});

test('6. Global search works', async ({ page }) => {
  await page.goto('/search?q=test');
  await expect(page).toHaveURL(/search/);
  await expect(page.locator('body')).toBeVisible();
});

test('7. API endpoint returns JSON for RadnaOprema', async ({ page }) => {
  const response = await page.request.get('/api/radnaOprema');
  expect(response.status()).toBe(200);
  const json = await response.json();
  expect(Array.isArray(json)).toBeTruthy();
});

test('8. API endpoint returns JSON for Radnik', async ({ page }) => {
  const response = await page.request.get('/api/radnik');
  expect(response.status()).toBe(200);
  const json = await response.json();
  expect(Array.isArray(json)).toBeTruthy();
});

test('9. API returns 404 for non-existing RadnaOprema', async ({ page }) => {
  const response = await page.request.get('/api/radnaOprema/99999');
  expect(response.status()).toBe(404);
});

test('10. Login page loads', async ({ page }) => {
  await page.goto('/Identity/Account/Login');
  await expect(page).toHaveURL(/Login/);
  await expect(page.locator('body')).toBeVisible();
});
